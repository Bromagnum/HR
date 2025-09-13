using BLL.DTOs;
using BLL.Utilities;

namespace BLL.Services;

/// <summary>
/// Payroll (Bordro) Service Interface
/// Bordro iş mantığı operasyonları için sözleşme
/// </summary>
public interface IPayrollService
{
    #region CRUD Operations (Temel işlemler)
    
    /// <summary>
    /// Tüm bordroları getirir
    /// </summary>
    Task<Result<IEnumerable<PayrollListDto>>> GetAllAsync();
    
    /// <summary>
    /// ID'ye göre bordro detayını getirir
    /// </summary>
    Task<Result<PayrollDetailDto>> GetByIdAsync(int id);
    
    /// <summary>
    /// Yeni bordro oluşturur
    /// </summary>
    Task<Result<PayrollDetailDto>> CreateAsync(PayrollCreateDto dto);
    
    /// <summary>
    /// Bordroyu günceller
    /// </summary>
    Task<Result<PayrollDetailDto>> UpdateAsync(PayrollUpdateDto dto);
    
    /// <summary>
    /// Bordroyu siler (soft delete)
    /// </summary>
    Task<Result<bool>> DeleteAsync(int id);
    
    #endregion

    #region Specialized Queries (Özel sorgular)
    
    /// <summary>
    /// Personelin bordro geçmişini getirir
    /// </summary>
    Task<Result<IEnumerable<PayrollListDto>>> GetByPersonIdAsync(int personId);
    
    /// <summary>
    /// Belirli yıldaki tüm bordroları getirir
    /// </summary>
    Task<Result<IEnumerable<PayrollListDto>>> GetByYearAsync(int year);
    
    /// <summary>
    /// Belirli dönemdeki (ay/yıl) tüm bordroları getirir
    /// </summary>
    Task<Result<IEnumerable<PayrollListDto>>> GetByPeriodAsync(int year, int month);
    
    /// <summary>
    /// Personelin belirli dönemdeki bordrosunu getirir
    /// </summary>
    Task<Result<PayrollDetailDto>> GetByPersonAndPeriodAsync(int personId, int year, int month);
    
    /// <summary>
    /// Departman bordroları getirir
    /// </summary>
    Task<Result<IEnumerable<PayrollListDto>>> GetByDepartmentAndPeriodAsync(int departmentId, int year, int month);
    
    #endregion

    #region Filtering & Search (Filtreleme ve arama)
    
    /// <summary>
    /// Filtreleme ile bordro arar
    /// </summary>
    Task<Result<IEnumerable<PayrollListDto>>> SearchAsync(PayrollFilterDto filter);
    
    #endregion

    #region Business Logic (İş mantığı)
    
    /// <summary>
    /// Net maaş hesaplar (Brüt - Kesinti)
    /// </summary>
    decimal CalculateNetSalary(decimal basicSalary, decimal allowances, decimal bonuses, decimal deductions);
    
    /// <summary>
    /// Brüt maaş hesaplar (Temel + Ek Ödemeler + İkramiye)
    /// </summary>
    decimal CalculateGrossSalary(decimal basicSalary, decimal allowances, decimal bonuses = 0);
    
    /// <summary>
    /// Aynı personel için aynı dönemde bordro var mı kontrol eder
    /// </summary>
    Task<Result<bool>> IsPayrollExistsForPeriodAsync(int personId, int year, int month, int? excludePayrollId = null);
    
    #endregion

    #region Reports (Raporlar)
    
    /// <summary>
    /// Dönem bordro özeti raporu
    /// </summary>
    Task<Result<PayrollSummaryDto>> GetPeriodSummaryAsync(int year, int month);
    
    /// <summary>
    /// Personelin yıllık bordro özeti
    /// </summary>
    Task<Result<PersonYearlyPayrollSummaryDto>> GetPersonYearlySummaryAsync(int personId, int year);
    
    /// <summary>
    /// Departman bordro özeti
    /// </summary>
    Task<Result<IEnumerable<PayrollListDto>>> GetDepartmentPayrollsAsync(int departmentId, int year, int? month = null);
    
    #endregion

    #region Validation (Doğrulama)
    
    /// <summary>
    /// Bordro oluşturma verilerini doğrular
    /// </summary>
    Task<Result<bool>> ValidatePayrollDataAsync(PayrollCreateDto dto);
    
    /// <summary>
    /// Bordro güncelleme verilerini doğrular
    /// </summary>
    Task<Result<bool>> ValidatePayrollUpdateAsync(PayrollUpdateDto dto);
    
    #endregion
}
