using BLL.DTOs;
using BLL.Utilities;

namespace BLL.Services.Export
{
    public interface IWordExportService
    {
        /// <summary>
        /// Personel raporu Word belgesi oluşturur
        /// </summary>
        /// <param name="personId">Personel ID</param>
        /// <returns>Word belgesi byte array</returns>
        Task<Result<byte[]>> GeneratePersonReportAsync(int personId);

        /// <summary>
        /// Departman raporu Word belgesi oluşturur
        /// </summary>
        /// <param name="departmentId">Departman ID</param>
        /// <returns>Word belgesi byte array</returns>
        Task<Result<byte[]>> GenerateDepartmentReportAsync(int departmentId);

        /// <summary>
        /// İzin raporu Word belgesi oluşturur
        /// </summary>
        /// <param name="startDate">Başlangıç tarihi</param>
        /// <param name="endDate">Bitiş tarihi</param>
        /// <param name="departmentId">Departman ID (opsiyonel)</param>
        /// <returns>Word belgesi byte array</returns>
        Task<Result<byte[]>> GenerateLeaveReportAsync(DateTime startDate, DateTime endDate, int? departmentId = null);

        /// <summary>
        /// Personel performans raporu Word belgesi oluşturur
        /// </summary>
        /// <param name="personId">Personel ID</param>
        /// <param name="startDate">Başlangıç tarihi</param>
        /// <param name="endDate">Bitiş tarihi</param>
        /// <returns>Word belgesi byte array</returns>
        Task<Result<byte[]>> GeneratePerformanceReportAsync(int personId, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Bordro Word belgesi oluşturur
        /// </summary>
        /// <param name="personId">Personel ID</param>
        /// <param name="year">Yıl</param>
        /// <param name="month">Ay</param>
        /// <returns>Word belgesi byte array</returns>
        Task<Result<byte[]>> GeneratePayrollReportAsync(int personId, int year, int month);

        /// <summary>
        /// Genel organizasyon raporu Word belgesi oluşturur
        /// </summary>
        /// <returns>Word belgesi byte array</returns>
        Task<Result<byte[]>> GenerateOrganizationReportAsync();
    }
}
