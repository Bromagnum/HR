using BLL.DTOs;

namespace BLL.Services.Export;

public interface IExcelExportService
{
    /// <summary>
    /// Departman listesini Excel formatında export eder
    /// </summary>
    /// <param name="departments">Export edilecek departmanlar</param>
    /// <param name="includeHierarchy">Hiyerarşik yapıyı dahil et</param>
    /// <returns>Excel dosyası byte array</returns>
    Task<byte[]> ExportDepartmentListAsync(IEnumerable<DepartmentListDto> departments, bool includeHierarchy = true);
    
    /// <summary>
    /// Organizasyon şemasını Excel formatında export eder
    /// </summary>
    /// <param name="departments">Tüm departmanlar</param>
    /// <returns>Excel dosyası byte array</returns>
    Task<byte[]> ExportOrganizationChartAsync(IEnumerable<DepartmentListDto> departments);
    
    /// <summary>
    /// Departman detay raporunu Excel formatında export eder
    /// </summary>
    /// <param name="departmentId">Departman ID</param>
    /// <param name="department">Departman detay bilgileri</param>
    /// <returns>Excel dosyası byte array</returns>
    Task<byte[]> ExportDepartmentDetailAsync(int departmentId, DepartmentDetailDto department);
    
    /// <summary>
    /// Filtrelenmiş departman arama sonuçlarını Excel formatında export eder
    /// </summary>
    /// <param name="searchResult">Arama sonuçları</param>
    /// <returns>Excel dosyası byte array</returns>
    Task<byte[]> ExportSearchResultsAsync(DepartmentSearchResultDto searchResult);
}

