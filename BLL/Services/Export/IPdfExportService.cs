using BLL.DTOs;

namespace BLL.Services.Export;

public interface IPdfExportService
{
    /// <summary>
    /// Personel listesini PDF formatında export eder
    /// </summary>
    /// <param name="personnel">Export edilecek personel listesi</param>
    /// <param name="includeDetails">Detay bilgileri dahil et</param>
    /// <returns>PDF dosyası byte array</returns>
    Task<byte[]> ExportPersonnelListAsync(IEnumerable<PersonListDto> personnel, bool includeDetails = true);
    
    /// <summary>
    /// Personel detay raporunu PDF formatında export eder
    /// </summary>
    /// <param name="personId">Personel ID</param>
    /// <param name="person">Personel detay bilgileri</param>
    /// <returns>PDF dosyası byte array</returns>
    Task<byte[]> ExportPersonnelDetailAsync(int personId, PersonDetailDto person);
    
    /// <summary>
    /// Departman listesini PDF formatında export eder
    /// </summary>
    /// <param name="departments">Export edilecek departmanlar</param>
    /// <param name="includeHierarchy">Hiyerarşik yapıyı dahil et</param>
    /// <returns>PDF dosyası byte array</returns>
    Task<byte[]> ExportDepartmentListAsync(IEnumerable<DepartmentListDto> departments, bool includeHierarchy = true);
    
    /// <summary>
    /// Organizasyon şemasını PDF formatında export eder
    /// </summary>
    /// <param name="departments">Tüm departmanlar</param>
    /// <returns>PDF dosyası byte array</returns>
    Task<byte[]> ExportOrganizationChartAsync(IEnumerable<DepartmentListDto> departments);
    
    /// <summary>
    /// İzin raporunu PDF formatında export eder
    /// </summary>
    /// <param name="leaves">İzin kayıtları</param>
    /// <param name="startDate">Başlangıç tarihi</param>
    /// <param name="endDate">Bitiş tarihi</param>
    /// <returns>PDF dosyası byte array</returns>
    Task<byte[]> ExportLeaveReportAsync(IEnumerable<LeaveListDto> leaves, DateTime? startDate = null, DateTime? endDate = null);
    
    /// <summary>
    /// İzin bakiye raporunu PDF formatında export eder
    /// </summary>
    /// <param name="leaveBalances">İzin bakiyeleri</param>
    /// <returns>PDF dosyası byte array</returns>
    Task<byte[]> ExportLeaveBalanceReportAsync(IEnumerable<LeaveBalanceListDto> leaveBalances);
    
    /// <summary>
    /// Malzeme stok raporunu PDF formatında export eder
    /// </summary>
    /// <param name="materials">Malzeme listesi</param>
    /// <param name="includeStockAnalysis">Stok analizi dahil et</param>
    /// <returns>PDF dosyası byte array</returns>
    Task<byte[]> ExportMaterialStockReportAsync(IEnumerable<MaterialListDto> materials, bool includeStockAnalysis = true);
    
    /// <summary>
    /// Pozisyon raporunu PDF formatında export eder
    /// </summary>
    /// <param name="positions">Pozisyon listesi</param>
    /// <returns>PDF dosyası byte array</returns>
    Task<byte[]> ExportPositionReportAsync(IEnumerable<PositionListDto> positions);
    
    /// <summary>
    /// Dashboard özet raporunu PDF formatında export eder
    /// </summary>
    /// <param name="summary">Dashboard özet verileri</param>
    /// <returns>PDF dosyası byte array</returns>
    Task<byte[]> ExportDashboardSummaryAsync(DashboardSummaryDto summary);
    
    /// <summary>
    /// Generic list export - Herhangi bir listeyi PDF formatında export eder
    /// </summary>
    /// <typeparam name="T">List tipi</typeparam>
    /// <param name="data">Export edilecek veri</param>
    /// <param name="title">Rapor başlığı</param>
    /// <param name="fileName">Dosya adı</param>
    /// <returns>PDF dosyası byte array</returns>
    Task<byte[]> ExportAsync<T>(IEnumerable<T> data, string title, string fileName) where T : class;
}
