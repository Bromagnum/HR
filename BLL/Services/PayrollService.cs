using AutoMapper;
using BLL.DTOs;
using BLL.Utilities;
using DAL.Entities;
using DAL.Repositories;

namespace BLL.Services;

/// <summary>
/// Payroll (Bordro) Service Implementation
/// Bordro iş mantığı operasyonlarını gerçekleştirir
/// </summary>
public class PayrollService : IPayrollService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PayrollService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    #region CRUD Operations

    public async Task<Result<IEnumerable<PayrollListDto>>> GetAllAsync()
    {
        try
        {
            var payrolls = await _unitOfWork.Payrolls.GetAllAsync();
            var payrollDtos = _mapper.Map<IEnumerable<PayrollListDto>>(payrolls);
            return Result<IEnumerable<PayrollListDto>>.Ok(payrollDtos);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<PayrollListDto>>.Fail($"Bordrolar getirilirken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<PayrollDetailDto>> GetByIdAsync(int id)
    {
        try
        {
            var payroll = await _unitOfWork.Payrolls.GetByIdAsync(id);
            if (payroll == null)
            {
                return Result<PayrollDetailDto>.Fail("Bordro bulunamadı.");
            }

            var payrollDto = _mapper.Map<PayrollDetailDto>(payroll);
            return Result<PayrollDetailDto>.Ok(payrollDto);
        }
        catch (Exception ex)
        {
            return Result<PayrollDetailDto>.Fail($"Bordro getirilirken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<PayrollDetailDto>> CreateAsync(PayrollCreateDto dto)
    {
        try
        {
            // Veri doğrulama
            var validationResult = await ValidatePayrollDataAsync(dto);
            if (!validationResult.IsSuccess)
            {
                return Result<PayrollDetailDto>.Fail(validationResult.Message);
            }

            // Aynı dönemde bordro var mı kontrol et
            var existsResult = await IsPayrollExistsForPeriodAsync(dto.PersonId, dto.Year, dto.Month);
            if (!existsResult.IsSuccess)
            {
                return Result<PayrollDetailDto>.Fail(existsResult.Message);
            }
            if (existsResult.Data)
            {
                return Result<PayrollDetailDto>.Fail($"Bu personel için {dto.Month}/{dto.Year} döneminde zaten bordro mevcut.");
            }

            // Entity'e dönüştür
            var payroll = _mapper.Map<Payroll>(dto);
            
            // Net maaş hesapla
            payroll.NetSalary = CalculateNetSalary(dto.BasicSalary, dto.Allowances, dto.Bonuses, dto.Deductions);
            
            // Bordro hazırlanma tarihi
            payroll.PreparedDate = DateTime.Now;

            // Veritabanına ekle
            await _unitOfWork.Payrolls.AddAsync(payroll);
            await _unitOfWork.SaveChangesAsync();

            // Geri döndür
            var createdPayroll = await _unitOfWork.Payrolls.GetByIdAsync(payroll.Id);
            var result = _mapper.Map<PayrollDetailDto>(createdPayroll);
            
            return Result<PayrollDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            return Result<PayrollDetailDto>.Fail($"Bordro oluşturulurken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<PayrollDetailDto>> UpdateAsync(PayrollUpdateDto dto)
    {
        try
        {
            // Veri doğrulama
            var validationResult = await ValidatePayrollUpdateAsync(dto);
            if (!validationResult.IsSuccess)
            {
                return Result<PayrollDetailDto>.Fail(validationResult.Message);
            }

            // Mevcut bordroyu bul
            var existingPayroll = await _unitOfWork.Payrolls.GetByIdAsync(dto.Id);
            if (existingPayroll == null)
            {
                return Result<PayrollDetailDto>.Fail("Güncellenecek bordro bulunamadı.");
            }

            // Aynı dönemde başka bordro var mı kontrol et (kendisi hariç)
            var existsResult = await IsPayrollExistsForPeriodAsync(dto.PersonId, dto.Year, dto.Month, dto.Id);
            if (!existsResult.IsSuccess)
            {
                return Result<PayrollDetailDto>.Fail(existsResult.Message);
            }
            if (existsResult.Data)
            {
                return Result<PayrollDetailDto>.Fail($"Bu personel için {dto.Month}/{dto.Year} döneminde başka bir bordro mevcut.");
            }

            // Güncelleme
            _mapper.Map(dto, existingPayroll);
            
            // Net maaş yeniden hesapla
            existingPayroll.NetSalary = CalculateNetSalary(dto.BasicSalary, dto.Allowances, dto.Bonuses, dto.Deductions);
            
            // Güncelleme tarihi
            existingPayroll.UpdatedAt = DateTime.Now;

            // Veritabanında güncelle
            _unitOfWork.Payrolls.Update(existingPayroll);
            await _unitOfWork.SaveChangesAsync();

            // Güncellenmiş halini getir
            var updatedPayroll = await _unitOfWork.Payrolls.GetByIdAsync(dto.Id);
            var result = _mapper.Map<PayrollDetailDto>(updatedPayroll);

            return Result<PayrollDetailDto>.Ok(result);
        }
        catch (Exception ex)
        {
            return Result<PayrollDetailDto>.Fail($"Bordro güncellenirken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<bool>> DeleteAsync(int id)
    {
        try
        {
            var payroll = await _unitOfWork.Payrolls.GetByIdAsync(id);
            if (payroll == null)
            {
                return Result<bool>.Fail("Silinecek bordro bulunamadı.");
            }

            // Soft delete
            payroll.IsActive = false;
            payroll.UpdatedAt = DateTime.Now;

            _unitOfWork.Payrolls.Update(payroll);
            await _unitOfWork.SaveChangesAsync();

            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Fail($"Bordro silinirken hata oluştu: {ex.Message}");
        }
    }

    #endregion

    #region Specialized Queries

    public async Task<Result<IEnumerable<PayrollListDto>>> GetByPersonIdAsync(int personId)
    {
        try
        {
            var payrolls = await _unitOfWork.Payrolls.GetByPersonIdAsync(personId);
            var payrollDtos = _mapper.Map<IEnumerable<PayrollListDto>>(payrolls);
            return Result<IEnumerable<PayrollListDto>>.Ok(payrollDtos);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<PayrollListDto>>.Fail($"Personel bordroları getirilirken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<PayrollListDto>>> GetByYearAsync(int year)
    {
        try
        {
            var payrolls = await _unitOfWork.Payrolls.GetByYearAsync(year);
            var payrollDtos = _mapper.Map<IEnumerable<PayrollListDto>>(payrolls);
            return Result<IEnumerable<PayrollListDto>>.Ok(payrollDtos);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<PayrollListDto>>.Fail($"Yıllık bordrolar getirilirken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<PayrollListDto>>> GetByPeriodAsync(int year, int month)
    {
        try
        {
            var payrolls = await _unitOfWork.Payrolls.GetByPeriodAsync(year, month);
            var payrollDtos = _mapper.Map<IEnumerable<PayrollListDto>>(payrolls);
            return Result<IEnumerable<PayrollListDto>>.Ok(payrollDtos);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<PayrollListDto>>.Fail($"Dönem bordroları getirilirken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<PayrollDetailDto>> GetByPersonAndPeriodAsync(int personId, int year, int month)
    {
        try
        {
            var payroll = await _unitOfWork.Payrolls.GetByPersonAndPeriodAsync(personId, year, month);
            if (payroll == null)
            {
                return Result<PayrollDetailDto>.Fail("Belirtilen dönemde bordro bulunamadı.");
            }

            var payrollDto = _mapper.Map<PayrollDetailDto>(payroll);
            return Result<PayrollDetailDto>.Ok(payrollDto);
        }
        catch (Exception ex)
        {
            return Result<PayrollDetailDto>.Fail($"Personel dönem bordrosu getirilirken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<PayrollListDto>>> GetByDepartmentAndPeriodAsync(int departmentId, int year, int month)
    {
        try
        {
            var payrolls = await _unitOfWork.Payrolls.GetByDepartmentAndPeriodAsync(departmentId, year, month);
            var payrollDtos = _mapper.Map<IEnumerable<PayrollListDto>>(payrolls);
            return Result<IEnumerable<PayrollListDto>>.Ok(payrollDtos);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<PayrollListDto>>.Fail($"Departman bordroları getirilirken hata oluştu: {ex.Message}");
        }
    }

    #endregion

    #region Search & Filter

    public async Task<Result<IEnumerable<PayrollListDto>>> SearchAsync(PayrollFilterDto filter)
    {
        try
        {
            var allPayrolls = await _unitOfWork.Payrolls.GetAllAsync();
            var query = allPayrolls.AsQueryable();

            // Filtreleme
            if (filter.PersonId.HasValue)
                query = query.Where(p => p.PersonId == filter.PersonId.Value);

            if (filter.DepartmentId.HasValue)
                query = query.Where(p => p.Person.DepartmentId == filter.DepartmentId.Value);

            if (filter.Year.HasValue)
                query = query.Where(p => p.Year == filter.Year.Value);

            if (filter.Month.HasValue)
                query = query.Where(p => p.Month == filter.Month.Value);

            if (filter.MinNetSalary.HasValue)
                query = query.Where(p => p.NetSalary >= filter.MinNetSalary.Value);

            if (filter.MaxNetSalary.HasValue)
                query = query.Where(p => p.NetSalary <= filter.MaxNetSalary.Value);

            // Sayfalama
            var pagedResults = query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToList();

            var payrollDtos = _mapper.Map<IEnumerable<PayrollListDto>>(pagedResults);
            return Result<IEnumerable<PayrollListDto>>.Ok(payrollDtos);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<PayrollListDto>>.Fail($"Bordro arama işleminde hata oluştu: {ex.Message}");
        }
    }

    #endregion

    #region Business Logic

    public decimal CalculateNetSalary(decimal basicSalary, decimal allowances, decimal bonuses, decimal deductions)
    {
        var grossSalary = CalculateGrossSalary(basicSalary, allowances, bonuses);
        var netSalary = grossSalary - deductions;
        
        // Net maaş negatif olamaz
        return netSalary < 0 ? 0 : netSalary;
    }

    public decimal CalculateGrossSalary(decimal basicSalary, decimal allowances, decimal bonuses = 0)
    {
        return basicSalary + allowances + bonuses;
    }

    public async Task<Result<bool>> IsPayrollExistsForPeriodAsync(int personId, int year, int month, int? excludePayrollId = null)
    {
        try
        {
            var exists = await _unitOfWork.Payrolls.ExistsForPeriodAsync(personId, year, month, excludePayrollId);
            return Result<bool>.Ok(exists);
        }
        catch (Exception ex)
        {
            return Result<bool>.Fail($"Bordro varlık kontrolünde hata oluştu: {ex.Message}");
        }
    }

    #endregion

    #region Reports

    public async Task<Result<PayrollSummaryDto>> GetPeriodSummaryAsync(int year, int month)
    {
        try
        {
            var payrolls = await _unitOfWork.Payrolls.GetByPeriodAsync(year, month);
            var payrollList = payrolls.ToList();

            if (!payrollList.Any())
            {
                return Result<PayrollSummaryDto>.Fail($"{month}/{year} döneminde bordro bulunamadı.");
            }

            var summary = new PayrollSummaryDto
            {
                Year = year,
                Month = month,
                MonthName = GetMonthName(month),
                PayrollPeriod = $"{GetMonthName(month)} {year}",
                TotalEmployees = payrollList.Count,
                TotalBasicSalary = payrollList.Sum(p => p.BasicSalary),
                TotalAllowances = payrollList.Sum(p => p.Allowances),
                TotalGrossSalary = payrollList.Sum(p => p.BasicSalary + p.Allowances + p.Bonuses),
                TotalDeductions = payrollList.Sum(p => p.Deductions),
                TotalNetSalary = payrollList.Sum(p => p.NetSalary),
                AverageNetSalary = payrollList.Average(p => p.NetSalary),
                MinNetSalary = payrollList.Min(p => p.NetSalary),
                MaxNetSalary = payrollList.Max(p => p.NetSalary)
            };

            return Result<PayrollSummaryDto>.Ok(summary);
        }
        catch (Exception ex)
        {
            return Result<PayrollSummaryDto>.Fail($"Dönem özet raporu oluşturulurken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<PersonYearlyPayrollSummaryDto>> GetPersonYearlySummaryAsync(int personId, int year)
    {
        try
        {
            var payrolls = await _unitOfWork.Payrolls.GetByPersonIdAsync(personId);
            var yearlyPayrolls = payrolls.Where(p => p.Year == year).ToList();

            if (!yearlyPayrolls.Any())
            {
                return Result<PersonYearlyPayrollSummaryDto>.Fail($"{year} yılında personel bordrosu bulunamadı.");
            }

            var firstPayroll = yearlyPayrolls.First();
            var summary = new PersonYearlyPayrollSummaryDto
            {
                PersonId = personId,
                PersonFullName = $"{firstPayroll.Person.FirstName} {firstPayroll.Person.LastName}",
                EmployeeNumber = firstPayroll.Person.EmployeeNumber ?? "",
                DepartmentName = firstPayroll.Person.Department?.Name ?? "",
                Year = year,
                PayrollCount = yearlyPayrolls.Count,
                YearlyBasicSalary = yearlyPayrolls.Sum(p => p.BasicSalary),
                YearlyAllowances = yearlyPayrolls.Sum(p => p.Allowances),
                YearlyGrossSalary = yearlyPayrolls.Sum(p => p.BasicSalary + p.Allowances),
                YearlyDeductions = yearlyPayrolls.Sum(p => p.Deductions),
                YearlyNetSalary = yearlyPayrolls.Sum(p => p.NetSalary),
                AverageMonthlyNet = yearlyPayrolls.Average(p => p.NetSalary)
            };

            return Result<PersonYearlyPayrollSummaryDto>.Ok(summary);
        }
        catch (Exception ex)
        {
            return Result<PersonYearlyPayrollSummaryDto>.Fail($"Personel yıllık özet raporu oluşturulurken hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<PayrollListDto>>> GetDepartmentPayrollsAsync(int departmentId, int year, int? month = null)
    {
        try
        {
            var allPayrolls = await _unitOfWork.Payrolls.GetByYearAsync(year);
            var departmentPayrolls = allPayrolls.Where(p => p.Person.DepartmentId == departmentId);

            if (month.HasValue)
            {
                departmentPayrolls = departmentPayrolls.Where(p => p.Month == month.Value);
            }

            var payrollDtos = _mapper.Map<IEnumerable<PayrollListDto>>(departmentPayrolls);
            return Result<IEnumerable<PayrollListDto>>.Ok(payrollDtos);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<PayrollListDto>>.Fail($"Departman bordroları getirilirken hata oluştu: {ex.Message}");
        }
    }

    #endregion

    #region Validation

    public async Task<Result<bool>> ValidatePayrollDataAsync(PayrollCreateDto dto)
    {
        try
        {
            // Personel var mı kontrol et
            var person = await _unitOfWork.Persons.GetByIdAsync(dto.PersonId);
            if (person == null)
            {
                return Result<bool>.Fail("Seçilen personel bulunamadı.");
            }

            // Tarih kontrolü
            if (dto.Year < 2020 || dto.Year > 2030)
            {
                return Result<bool>.Fail("Yıl 2020-2030 arasında olmalıdır.");
            }

            if (dto.Month < 1 || dto.Month > 12)
            {
                return Result<bool>.Fail("Ay 1-12 arasında olmalıdır.");
            }

            // Maaş kontrolü
            if (dto.BasicSalary < 0)
            {
                return Result<bool>.Fail("Temel maaş negatif olamaz.");
            }

            if (dto.Allowances < 0)
            {
                return Result<bool>.Fail("Ek ödemeler negatif olamaz.");
            }

            if (dto.Deductions < 0)
            {
                return Result<bool>.Fail("Kesintiler negatif olamaz.");
            }

            // Net maaş kontrolü
            var netSalary = CalculateNetSalary(dto.BasicSalary, dto.Allowances, dto.Bonuses, dto.Deductions);
            if (netSalary <= 0)
            {
                return Result<bool>.Fail("Net maaş sıfır veya negatif olamaz. Kesintileri kontrol edin.");
            }

            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Fail($"Veri doğrulama sırasında hata oluştu: {ex.Message}");
        }
    }

    public async Task<Result<bool>> ValidatePayrollUpdateAsync(PayrollUpdateDto dto)
    {
        try
        {
            // Personel var mı kontrol et
            var person = await _unitOfWork.Persons.GetByIdAsync(dto.PersonId);
            if (person == null)
            {
                return Result<bool>.Fail("Seçilen personel bulunamadı.");
            }

            // Tarih kontrolü
            if (dto.Year < 2020 || dto.Year > 2030)
            {
                return Result<bool>.Fail("Yıl 2020-2030 arasında olmalıdır.");
            }

            if (dto.Month < 1 || dto.Month > 12)
            {
                return Result<bool>.Fail("Ay 1-12 arasında olmalıdır.");
            }

            // Maaş kontrolü
            if (dto.BasicSalary < 0)
            {
                return Result<bool>.Fail("Temel maaş negatif olamaz.");
            }

            if (dto.Allowances < 0)
            {
                return Result<bool>.Fail("Ek ödemeler negatif olamaz.");
            }

            if (dto.Deductions < 0)
            {
                return Result<bool>.Fail("Kesintiler negatif olamaz.");
            }

            // Net maaş kontrolü
            var netSalary = CalculateNetSalary(dto.BasicSalary, dto.Allowances, dto.Bonuses, dto.Deductions);
            if (netSalary <= 0)
            {
                return Result<bool>.Fail("Net maaş sıfır veya negatif olamaz. Kesintileri kontrol edin.");
            }

            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Fail($"Güncelleme verisi doğrulama sırasında hata oluştu: {ex.Message}");
        }
    }

    #endregion

    #region Helper Methods

    private static string GetMonthName(int month)
    {
        return month switch
        {
            1 => "Ocak", 2 => "Şubat", 3 => "Mart", 4 => "Nisan",
            5 => "Mayıs", 6 => "Haziran", 7 => "Temmuz", 8 => "Ağustos",
            9 => "Eylül", 10 => "Ekim", 11 => "Kasım", 12 => "Aralık",
            _ => "Bilinmiyor"
        };
    }

    #endregion
}
