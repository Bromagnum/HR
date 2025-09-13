using DAL.Entities;

namespace DAL.Repositories;

/// <summary>
/// Payroll (Bordro) Repository Interface
/// Bordro verilerine erişim için sözleşme tanımlar
/// </summary>
public interface IPayrollRepository : IRepository<Payroll>
{
    /// <summary>
    /// Belirli bir personelin belirli ay/yıldaki bordrosunu getirir
    /// </summary>
    /// <param name="personId">Personel ID</param>
    /// <param name="year">Yıl</param>
    /// <param name="month">Ay (1-12)</param>
    /// <returns>Bordro kaydı</returns>
    Task<Payroll?> GetByPersonAndPeriodAsync(int personId, int year, int month);
    
    /// <summary>
    /// Belirli bir personelin tüm bordrolarını getirir
    /// </summary>
    /// <param name="personId">Personel ID</param>
    /// <returns>Bordro listesi</returns>
    Task<IEnumerable<Payroll>> GetByPersonIdAsync(int personId);
    
    /// <summary>
    /// Belirli bir yıldaki tüm bordroları getirir
    /// </summary>
    /// <param name="year">Yıl</param>
    /// <returns>Bordro listesi</returns>
    Task<IEnumerable<Payroll>> GetByYearAsync(int year);
    
    /// <summary>
    /// Belirli bir ay/yıldaki tüm bordroları getirir
    /// </summary>
    /// <param name="year">Yıl</param>
    /// <param name="month">Ay (1-12)</param>
    /// <returns>Bordro listesi</returns>
    Task<IEnumerable<Payroll>> GetByPeriodAsync(int year, int month);
    
    /// <summary>
    /// Belirli bir departmanın belirli ay/yıldaki bordrolarını getirir
    /// </summary>
    /// <param name="departmentId">Departman ID</param>
    /// <param name="year">Yıl</param>
    /// <param name="month">Ay (1-12)</param>
    /// <returns>Bordro listesi</returns>
    Task<IEnumerable<Payroll>> GetByDepartmentAndPeriodAsync(int departmentId, int year, int month);
    
    /// <summary>
    /// Bordro maaş toplamlarını getirir (raporlama için)
    /// </summary>
    /// <param name="year">Yıl</param>
    /// <param name="month">Ay (1-12)</param>
    /// <returns>Toplam brüt maaş, toplam net maaş, toplam kesinti</returns>
    Task<(decimal TotalGross, decimal TotalNet, decimal TotalDeductions)> GetPeriodSummaryAsync(int year, int month);
    
    /// <summary>
    /// Personelin yıllık maaş özeti
    /// </summary>
    /// <param name="personId">Personel ID</param>
    /// <param name="year">Yıl</param>
    /// <returns>Yıllık toplam maaş bilgileri</returns>
    Task<(decimal YearlyGross, decimal YearlyNet, decimal YearlyDeductions, int MonthCount)> GetPersonYearlySummaryAsync(int personId, int year);
    
    /// <summary>
    /// Aynı personel için aynı ay/yıl kontrolü
    /// </summary>
    /// <param name="personId">Personel ID</param>
    /// <param name="year">Yıl</param>
    /// <param name="month">Ay</param>
    /// <param name="excludePayrollId">Hariç tutulacak bordro ID (güncelleme için)</param>
    /// <returns>Var mı kontrolü</returns>
    Task<bool> ExistsForPeriodAsync(int personId, int year, int month, int? excludePayrollId = null);
}
