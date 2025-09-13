using DAL.Context;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

/// <summary>
/// Payroll (Bordro) Repository Implementation
/// Bordro verilerine erişim işlemlerini gerçekleştirir
/// </summary>
public class PayrollRepository : Repository<Payroll>, IPayrollRepository
{
    public PayrollRepository(AppDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Belirli bir personelin belirli ay/yıldaki bordrosunu getirir
    /// </summary>
    public async Task<Payroll?> GetByPersonAndPeriodAsync(int personId, int year, int month)
    {
        return await _context.Payrolls
            .Include(p => p.Person)
                .ThenInclude(per => per.Department)
            .Include(p => p.Person)
                .ThenInclude(per => per.Position)
            .Include(p => p.PreparedBy)
            .FirstOrDefaultAsync(p => p.PersonId == personId && 
                                    p.Year == year && 
                                    p.Month == month && 
                                    p.IsActive);
    }

    /// <summary>
    /// Belirli bir personelin tüm bordrolarını getirir
    /// </summary>
    public async Task<IEnumerable<Payroll>> GetByPersonIdAsync(int personId)
    {
        return await _context.Payrolls
            .Include(p => p.Person)
                .ThenInclude(per => per.Department)
            .Include(p => p.Person)
                .ThenInclude(per => per.Position)
            .Include(p => p.PreparedBy)
            .Where(p => p.PersonId == personId && p.IsActive)
            .OrderByDescending(p => p.Year)
            .ThenByDescending(p => p.Month)
            .ToListAsync();
    }

    /// <summary>
    /// Belirli bir yıldaki tüm bordroları getirir
    /// </summary>
    public async Task<IEnumerable<Payroll>> GetByYearAsync(int year)
    {
        return await _context.Payrolls
            .Include(p => p.Person)
                .ThenInclude(per => per.Department)
            .Include(p => p.Person)
                .ThenInclude(per => per.Position)
            .Include(p => p.PreparedBy)
            .Where(p => p.Year == year && p.IsActive)
            .OrderBy(p => p.Month)
            .ThenBy(p => p.Person.FirstName)
            .ThenBy(p => p.Person.LastName)
            .ToListAsync();
    }

    /// <summary>
    /// Belirli bir ay/yıldaki tüm bordroları getirir
    /// </summary>
    public async Task<IEnumerable<Payroll>> GetByPeriodAsync(int year, int month)
    {
        return await _context.Payrolls
            .Include(p => p.Person)
                .ThenInclude(per => per.Department)
            .Include(p => p.Person)
                .ThenInclude(per => per.Position)
            .Include(p => p.PreparedBy)
            .Where(p => p.Year == year && p.Month == month && p.IsActive)
            .OrderBy(p => p.Person.Department != null ? p.Person.Department.Name : "")
            .ThenBy(p => p.Person.FirstName)
            .ThenBy(p => p.Person.LastName)
            .ToListAsync();
    }

    /// <summary>
    /// Belirli bir departmanın belirli ay/yıldaki bordrolarını getirir
    /// </summary>
    public async Task<IEnumerable<Payroll>> GetByDepartmentAndPeriodAsync(int departmentId, int year, int month)
    {
        return await _context.Payrolls
            .Include(p => p.Person)
                .ThenInclude(per => per.Department)
            .Include(p => p.Person)
                .ThenInclude(per => per.Position)
            .Include(p => p.PreparedBy)
            .Where(p => p.Person.DepartmentId == departmentId && 
                       p.Year == year && 
                       p.Month == month && 
                       p.IsActive)
            .OrderBy(p => p.Person.FirstName)
            .ThenBy(p => p.Person.LastName)
            .ToListAsync();
    }

    /// <summary>
    /// Bordro maaş toplamlarını getirir (raporlama için)
    /// </summary>
    public async Task<(decimal TotalGross, decimal TotalNet, decimal TotalDeductions)> GetPeriodSummaryAsync(int year, int month)
    {
        var summary = await _context.Payrolls
            .Where(p => p.Year == year && p.Month == month && p.IsActive)
            .GroupBy(p => 1) // Tüm kayıtları gruplama
            .Select(g => new
            {
                TotalGross = g.Sum(p => p.BasicSalary + p.Allowances + p.Bonuses),
                TotalNet = g.Sum(p => p.NetSalary),
                TotalDeductions = g.Sum(p => p.Deductions)
            })
            .FirstOrDefaultAsync();

        return summary != null 
            ? (summary.TotalGross, summary.TotalNet, summary.TotalDeductions)
            : (0, 0, 0);
    }

    /// <summary>
    /// Personelin yıllık maaş özeti
    /// </summary>
    public async Task<(decimal YearlyGross, decimal YearlyNet, decimal YearlyDeductions, int MonthCount)> GetPersonYearlySummaryAsync(int personId, int year)
    {
        var summary = await _context.Payrolls
            .Where(p => p.PersonId == personId && p.Year == year && p.IsActive)
            .GroupBy(p => p.PersonId)
            .Select(g => new
            {
                YearlyGross = g.Sum(p => p.BasicSalary + p.Allowances + p.Bonuses),
                YearlyNet = g.Sum(p => p.NetSalary),
                YearlyDeductions = g.Sum(p => p.Deductions),
                MonthCount = g.Count()
            })
            .FirstOrDefaultAsync();

        return summary != null 
            ? (summary.YearlyGross, summary.YearlyNet, summary.YearlyDeductions, summary.MonthCount)
            : (0, 0, 0, 0);
    }

    /// <summary>
    /// Aynı personel için aynı ay/yıl kontrolü
    /// </summary>
    public async Task<bool> ExistsForPeriodAsync(int personId, int year, int month, int? excludePayrollId = null)
    {
        var query = _context.Payrolls
            .Where(p => p.PersonId == personId && 
                       p.Year == year && 
                       p.Month == month && 
                       p.IsActive);

        if (excludePayrollId.HasValue)
        {
            query = query.Where(p => p.Id != excludePayrollId.Value);
        }

        return await query.AnyAsync();
    }

    /// <summary>
    /// Genel GetAllAsync metodunu override ediyoruz - personel bilgileri ile birlikte getirmek için
    /// </summary>
    public override async Task<IEnumerable<Payroll>> GetAllAsync()
    {
        return await _context.Payrolls
            .Include(p => p.Person)
                .ThenInclude(per => per.Department)
            .Include(p => p.Person)
                .ThenInclude(per => per.Position)
            .Include(p => p.PreparedBy)
            .Where(p => p.IsActive)
            .OrderByDescending(p => p.Year)
            .ThenByDescending(p => p.Month)
            .ThenBy(p => p.Person.FirstName)
            .ThenBy(p => p.Person.LastName)
            .ToListAsync();
    }

    /// <summary>
    /// Genel GetByIdAsync metodunu override ediyoruz - personel bilgileri ile birlikte getirmek için
    /// </summary>
    public override async Task<Payroll?> GetByIdAsync(int id)
    {
        return await _context.Payrolls
            .Include(p => p.Person)
                .ThenInclude(per => per.Department)
            .Include(p => p.Person)
                .ThenInclude(per => per.Position)
            .Include(p => p.PreparedBy)
            .FirstOrDefaultAsync(p => p.Id == id && p.IsActive);
    }
}
