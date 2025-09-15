using AutoMapper;
using BLL.DTOs;
using BLL.Services.Export;
using BLL.Utilities;
using DAL.Entities;
using DAL.Repositories;
using Microsoft.Extensions.Logging;

namespace BLL.Services;

public class JobApplicationService : IJobApplicationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<JobApplicationService> _logger;
    private readonly IExcelExportService _excelExportService;

    public JobApplicationService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<JobApplicationService> logger,
        IExcelExportService excelExportService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
        _excelExportService = excelExportService;
    }

    #region CRUD Operations

    public async Task<Result<IEnumerable<JobApplicationListDto>>> GetAllAsync()
    {
        try
        {
            var applications = await _unitOfWork.JobApplications.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<JobApplicationListDto>>(applications);
            return Result<IEnumerable<JobApplicationListDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all job applications");
            return Result<IEnumerable<JobApplicationListDto>>.Fail($"İş başvuruları getirilemedi: {ex.Message}");
        }
    }

    public async Task<Result<JobApplicationDetailDto?>> GetByIdAsync(int id)
    {
        try
        {
            if (id <= 0)
                return Result<JobApplicationDetailDto?>.Fail("Geçersiz başvuru ID'si");

            var application = await _unitOfWork.JobApplications.GetByIdAsync(id);
            if (application == null)
                return Result<JobApplicationDetailDto?>.Fail("Başvuru bulunamadı");

            var dto = _mapper.Map<JobApplicationDetailDto>(application);
            return Result<JobApplicationDetailDto?>.Ok(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving job application with id {Id}", id);
            return Result<JobApplicationDetailDto?>.Fail($"Başvuru getirilemedi: {ex.Message}");
        }
    }

    public async Task<Result<JobApplicationDetailDto>> CreateAsync(JobApplicationCreateDto dto)
    {
        try
        {
            // Validation
            var validationResult = await ValidateCreateAsync(dto);
            if (!validationResult.IsSuccess)
                return Result<JobApplicationDetailDto>.Fail(validationResult.Message);

            // Check for duplicate application
            var hasApplication = await HasApplicationForPositionAsync(dto.Email, dto.PositionId);
            if (hasApplication.IsSuccess && hasApplication.Data)
                return Result<JobApplicationDetailDto>.Fail("Bu pozisyon için zaten başvurunuz bulunmaktadır");

            // Map and create
            var application = _mapper.Map<JobApplication>(dto);
            application.ApplicationDate = DateTime.Now;
            application.Status = JobApplicationStatus.Submitted;

            await _unitOfWork.JobApplications.AddAsync(application);
            await _unitOfWork.SaveChangesAsync();

            // Get with related data
            var result = await _unitOfWork.JobApplications.GetWithDocumentsAsync(application.Id);
            var resultDto = _mapper.Map<JobApplicationDetailDto>(result);

            _logger.LogInformation("Job application created successfully with id {Id}", application.Id);
            return Result<JobApplicationDetailDto>.Ok(resultDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating job application");
            return Result<JobApplicationDetailDto>.Fail($"Başvuru oluşturulamadı: {ex.Message}");
        }
    }

    public async Task<Result<JobApplicationDetailDto>> UpdateAsync(JobApplicationUpdateDto dto)
    {
        try
        {
            if (dto.Id <= 0)
                return Result<JobApplicationDetailDto>.Fail("Geçersiz başvuru ID'si");

            var existingApplication = await _unitOfWork.JobApplications.GetByIdAsync(dto.Id);
            if (existingApplication == null)
                return Result<JobApplicationDetailDto>.Fail("Başvuru bulunamadı");

            // Map updates
            _mapper.Map(dto, existingApplication);
            existingApplication.UpdatedAt = DateTime.Now;
            existingApplication.ReviewedAt = DateTime.Now;

            _unitOfWork.JobApplications.Update(existingApplication);
            await _unitOfWork.SaveChangesAsync();

            var updatedApplication = await _unitOfWork.JobApplications.GetWithDocumentsAsync(dto.Id);
            var resultDto = _mapper.Map<JobApplicationDetailDto>(updatedApplication);

            _logger.LogInformation("Job application updated successfully with id {Id}", dto.Id);
            return Result<JobApplicationDetailDto>.Ok(resultDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating job application with id {Id}", dto.Id);
            return Result<JobApplicationDetailDto>.Fail($"Başvuru güncellenemedi: {ex.Message}");
        }
    }

    public async Task<Result<bool>> DeleteAsync(int id)
    {
        try
        {
            if (id <= 0)
                return Result<bool>.Fail("Geçersiz başvuru ID'si");

            var application = await _unitOfWork.JobApplications.GetByIdAsync(id);
            if (application == null)
                return Result<bool>.Fail("Başvuru bulunamadı");

            _unitOfWork.JobApplications.Remove(application);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Job application deleted successfully with id {Id}", id);
            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting job application with id {Id}", id);
            return Result<bool>.Fail($"Başvuru silinemedi: {ex.Message}");
        }
    }

    public async Task<Result<bool>> SoftDeleteAsync(int id)
    {
        try
        {
            if (id <= 0)
                return Result<bool>.Fail("Geçersiz başvuru ID'si");

            var application = await _unitOfWork.JobApplications.GetByIdAsync(id);
            if (application == null)
                return Result<bool>.Fail("Başvuru bulunamadı");

            application.IsActive = false;
            application.UpdatedAt = DateTime.Now;

            _unitOfWork.JobApplications.Update(application);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Job application soft deleted successfully with id {Id}", id);
            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error soft deleting job application with id {Id}", id);
            return Result<bool>.Fail($"Başvuru pasif edilemedi: {ex.Message}");
        }
    }

    #endregion

    #region Filtering & Search

    public async Task<Result<IEnumerable<JobApplicationListDto>>> GetFilteredAsync(JobApplicationFilterDto filter)
    {
        try
        {
            var applications = await _unitOfWork.JobApplications.GetFilteredAsync(
                filter.Status,
                filter.PositionId,
                filter.StartDate,
                filter.EndDate,
                filter.SearchTerm,
                filter.ReviewedById);

            var dtos = _mapper.Map<IEnumerable<JobApplicationListDto>>(applications);
            return Result<IEnumerable<JobApplicationListDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error filtering job applications");
            return Result<IEnumerable<JobApplicationListDto>>.Fail($"Başvurular filtrelenemiyor: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<JobApplicationListDto>>> SearchAsync(string searchTerm)
    {
        try
        {
            var applications = await _unitOfWork.JobApplications.SearchAsync(searchTerm);
            var dtos = _mapper.Map<IEnumerable<JobApplicationListDto>>(applications);
            return Result<IEnumerable<JobApplicationListDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching job applications with term {SearchTerm}", searchTerm);
            return Result<IEnumerable<JobApplicationListDto>>.Fail($"Başvuru araması yapılamadı: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<JobApplicationListDto>>> GetByStatusAsync(JobApplicationStatus status)
    {
        try
        {
            var applications = await _unitOfWork.JobApplications.GetByStatusAsync(status);
            var dtos = _mapper.Map<IEnumerable<JobApplicationListDto>>(applications);
            return Result<IEnumerable<JobApplicationListDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving job applications by status {Status}", status);
            return Result<IEnumerable<JobApplicationListDto>>.Fail($"Başvurular getirilemedi: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<JobApplicationListDto>>> GetByPositionIdAsync(int positionId)
    {
        try
        {
            var applications = await _unitOfWork.JobApplications.GetByPositionIdAsync(positionId);
            var dtos = _mapper.Map<IEnumerable<JobApplicationListDto>>(applications);
            return Result<IEnumerable<JobApplicationListDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving job applications by position {PositionId}", positionId);
            return Result<IEnumerable<JobApplicationListDto>>.Fail($"Pozisyon başvuruları getirilemedi: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<JobApplicationListDto>>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        try
        {
            var applications = await _unitOfWork.JobApplications.GetByDateRangeAsync(startDate, endDate);
            var dtos = _mapper.Map<IEnumerable<JobApplicationListDto>>(applications);
            return Result<IEnumerable<JobApplicationListDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving job applications by date range");
            return Result<IEnumerable<JobApplicationListDto>>.Fail($"Tarih aralığı başvuruları getirilemedi: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<JobApplicationListDto>>> GetPendingApplicationsAsync()
    {
        try
        {
            var applications = await _unitOfWork.JobApplications.GetPendingApplicationsAsync();
            var dtos = _mapper.Map<IEnumerable<JobApplicationListDto>>(applications);
            return Result<IEnumerable<JobApplicationListDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving pending job applications");
            return Result<IEnumerable<JobApplicationListDto>>.Fail($"Bekleyen başvurular getirilemedi: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<JobApplicationListDto>>> GetRecentApplicationsAsync(int count = 10)
    {
        try
        {
            var applications = await _unitOfWork.JobApplications.GetRecentApplicationsAsync(count);
            var dtos = _mapper.Map<IEnumerable<JobApplicationListDto>>(applications);
            return Result<IEnumerable<JobApplicationListDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving recent job applications");
            return Result<IEnumerable<JobApplicationListDto>>.Fail($"Son başvurular getirilemedi: {ex.Message}");
        }
    }

    #endregion

    #region Status Management

    public async Task<Result<bool>> UpdateStatusAsync(int id, JobApplicationStatus status, int reviewedById, string? notes = null)
    {
        try
        {
            var application = await _unitOfWork.JobApplications.GetByIdAsync(id);
            if (application == null)
                return Result<bool>.Fail("Başvuru bulunamadı");

            application.Status = status;
            application.ReviewedById = reviewedById;
            application.ReviewedAt = DateTime.Now;
            application.UpdatedAt = DateTime.Now;

            if (!string.IsNullOrEmpty(notes))
                application.ReviewNotes = notes;

            _unitOfWork.JobApplications.Update(application);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Job application status updated to {Status} for id {Id} by user {ReviewedById}", 
                status, id, reviewedById);
            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating job application status");
            return Result<bool>.Fail($"Başvuru durumu güncellenemedi: {ex.Message}");
        }
    }

    public async Task<Result<bool>> ChangeStatusAsync(int id, JobApplicationStatus status, int reviewedById, string? notes = null)
    {
        return await UpdateStatusAsync(id, status, reviewedById, notes);
    }

    public async Task<Result<bool>> ApproveApplicationAsync(int id, int reviewedById, string? notes = null)
    {
        return await UpdateStatusAsync(id, JobApplicationStatus.Approved, reviewedById, notes);
    }

    public async Task<Result<bool>> RejectApplicationAsync(int id, int reviewedById, string? notes = null)
    {
        return await UpdateStatusAsync(id, JobApplicationStatus.Rejected, reviewedById, notes);
    }

    public async Task<Result<bool>> ScheduleInterviewAsync(int id, DateTime interviewDate, int reviewedById, string? notes = null)
    {
        try
        {
            var application = await _unitOfWork.JobApplications.GetByIdAsync(id);
            if (application == null)
                return Result<bool>.Fail("Başvuru bulunamadı");

            application.Status = JobApplicationStatus.Interviewed;
            application.InterviewDate = interviewDate;
            application.ReviewedById = reviewedById;
            application.ReviewedAt = DateTime.Now;
            application.UpdatedAt = DateTime.Now;

            if (!string.IsNullOrEmpty(notes))
                application.InterviewNotes = notes;

            _unitOfWork.JobApplications.Update(application);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Interview scheduled for application {Id} on {InterviewDate}", id, interviewDate);
            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error scheduling interview for application {Id}", id);
            return Result<bool>.Fail($"Mülakat planlanamadı: {ex.Message}");
        }
    }

    public async Task<Result<bool>> SetRatingAsync(int id, int rating, int reviewedById, string? notes = null)
    {
        try
        {
            if (rating < 1 || rating > 10)
                return Result<bool>.Fail("Değerlendirme 1-10 arasında olmalıdır");

            var application = await _unitOfWork.JobApplications.GetByIdAsync(id);
            if (application == null)
                return Result<bool>.Fail("Başvuru bulunamadı");

            application.Rating = rating;
            application.ReviewedById = reviewedById;
            application.ReviewedAt = DateTime.Now;
            application.UpdatedAt = DateTime.Now;

            if (!string.IsNullOrEmpty(notes))
                application.ReviewNotes = notes;

            _unitOfWork.JobApplications.Update(application);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Rating {Rating} set for application {Id} by user {ReviewedById}", 
                rating, id, reviewedById);
            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error setting rating for application {Id}", id);
            return Result<bool>.Fail($"Değerlendirme puanı verilemedi: {ex.Message}");
        }
    }

    #endregion

    #region Document Management

    public async Task<Result<IEnumerable<ApplicationDocumentDto>>> GetApplicationDocumentsAsync(int applicationId)
    {
        try
        {
            var application = await _unitOfWork.JobApplications.GetWithDocumentsAsync(applicationId);
            if (application == null)
                return Result<IEnumerable<ApplicationDocumentDto>>.Fail("Başvuru bulunamadı");

            var dtos = _mapper.Map<IEnumerable<ApplicationDocumentDto>>(application.Documents);
            return Result<IEnumerable<ApplicationDocumentDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving documents for application {ApplicationId}", applicationId);
            return Result<IEnumerable<ApplicationDocumentDto>>.Fail($"Belgeler getirilemedi: {ex.Message}");
        }
    }

    public async Task<Result<bool>> HasRequiredDocumentsAsync(int applicationId)
    {
        try
        {
            var application = await _unitOfWork.JobApplications.GetWithDocumentsAsync(applicationId);
            if (application == null)
                return Result<bool>.Fail("Başvuru bulunamadı");

            // Check if CV exists
            var hasCV = application.Documents.Any(d => d.DocumentType == DocumentType.CV);
            return Result<bool>.Ok(hasCV);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking required documents for application {ApplicationId}", applicationId);
            return Result<bool>.Fail($"Belge kontrolü yapılamadı: {ex.Message}");
        }
    }

    public async Task<Result<bool>> VerifyDocumentAsync(int documentId, int verifiedById, string? notes = null)
    {
        try
        {
            // This would be implemented when ApplicationDocument repository is available
            // For now, return success
            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error verifying document {DocumentId}", documentId);
            return Result<bool>.Fail($"Belge doğrulanamadı: {ex.Message}");
        }
    }

    #endregion

    #region Validation & Business Rules

    public async Task<Result<bool>> CanApplyForPositionAsync(string email, int positionId)
    {
        try
        {
            // Check if position exists and is active
            var positionResult = await IsPositionStillOpenAsync(positionId);
            if (!positionResult.IsSuccess || !positionResult.Data)
                return Result<bool>.Fail("Pozisyon artık başvuruya açık değil");

            // Check if already applied
            var hasApplication = await HasApplicationForPositionAsync(email, positionId);
            if (hasApplication.IsSuccess && hasApplication.Data)
                return Result<bool>.Fail("Bu pozisyon için zaten başvurunuz bulunmaktadır");

            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if can apply for position");
            return Result<bool>.Fail($"Başvuru kontrolü yapılamadı: {ex.Message}");
        }
    }

    public async Task<Result<bool>> HasApplicationForPositionAsync(string email, int positionId)
    {
        try
        {
            var hasApplication = await _unitOfWork.JobApplications.HasApplicationForPositionAsync(email, positionId);
            return Result<bool>.Ok(hasApplication);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking existing application");
            return Result<bool>.Fail($"Başvuru kontrolü yapılamadı: {ex.Message}");
        }
    }

    public async Task<Result<bool>> IsPositionStillOpenAsync(int positionId)
    {
        try
        {
            var position = await _unitOfWork.Positions.GetByIdAsync(positionId);
            return Result<bool>.Ok(position != null && position.IsActive && position.IsAvailable);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking position availability");
            return Result<bool>.Fail($"Pozisyon kontrolü yapılamadı: {ex.Message}");
        }
    }

    #endregion

    #region Statistics & Reports

    public async Task<Result<JobApplicationSummaryDto>> GetApplicationSummaryAsync()
    {
        try
        {
            var applications = await _unitOfWork.JobApplications.GetAllAsync();
            var summary = new JobApplicationSummaryDto
            {
                TotalApplications = applications.Count(),
                PendingApplications = applications.Count(a => a.Status == JobApplicationStatus.Submitted),
                UnderReviewApplications = applications.Count(a => a.Status == JobApplicationStatus.UnderReview),
                ShortlistedApplications = applications.Count(a => a.Status == JobApplicationStatus.UnderReview),
                InterviewApplications = applications.Count(a => a.Status == JobApplicationStatus.Interviewed),
                ApprovedApplications = applications.Count(a => a.Status == JobApplicationStatus.Approved),
                RejectedApplications = applications.Count(a => a.Status == JobApplicationStatus.Rejected)
            };

            return Result<JobApplicationSummaryDto>.Ok(summary);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating application summary");
            return Result<JobApplicationSummaryDto>.Fail($"İstatistik oluşturulamadı: {ex.Message}");
        }
    }

    public async Task<Result<JobApplicationSummaryDto>> GetSummaryAsync()
    {
        return await GetApplicationSummaryAsync();
    }

    public async Task<Result<JobApplicationSummaryDto>> GetApplicationSummaryByPeriodAsync(DateTime startDate, DateTime endDate)
    {
        try
        {
            var applications = await _unitOfWork.JobApplications.GetByDateRangeAsync(startDate, endDate);
            var summary = new JobApplicationSummaryDto
            {
                TotalApplications = applications.Count(),
                PendingApplications = applications.Count(a => a.Status == JobApplicationStatus.Submitted),
                UnderReviewApplications = applications.Count(a => a.Status == JobApplicationStatus.UnderReview),
                ShortlistedApplications = applications.Count(a => a.Status == JobApplicationStatus.UnderReview),
                InterviewApplications = applications.Count(a => a.Status == JobApplicationStatus.Interviewed),
                ApprovedApplications = applications.Count(a => a.Status == JobApplicationStatus.Approved),
                RejectedApplications = applications.Count(a => a.Status == JobApplicationStatus.Rejected)
            };

            return Result<JobApplicationSummaryDto>.Ok(summary);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating application summary by period");
            return Result<JobApplicationSummaryDto>.Fail($"Dönemsel istatistik oluşturulamadı: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<PositionApplicationSummaryDto>>> GetApplicationSummaryByPositionAsync()
    {
        try
        {
            var applications = await _unitOfWork.JobApplications.GetAllAsync();
            var positionSummary = applications
                .GroupBy(a => a.Position)
                .Select(g => new PositionApplicationSummaryDto
                {
                    PositionId = g.Key.Id,
                    PositionName = g.Key.Name,
                    DepartmentName = g.Key.Department?.Name ?? "",
                    ApplicationCount = g.Count(),
                    PendingCount = g.Count(a => a.Status == JobApplicationStatus.Submitted),
                    ApprovedCount = g.Count(a => a.Status == JobApplicationStatus.Approved),
                    RejectedCount = g.Count(a => a.Status == JobApplicationStatus.Rejected),
                    ApprovalRate = g.Count() > 0 ? (decimal)g.Count(a => a.Status == JobApplicationStatus.Approved) / g.Count() * 100 : 0
                })
                .ToList();

            return Result<IEnumerable<PositionApplicationSummaryDto>>.Ok(positionSummary);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating application summary by position");
            return Result<IEnumerable<PositionApplicationSummaryDto>>.Fail($"Pozisyon istatistikleri oluşturulamadı: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<PositionApplicationSummaryDto>>> GetPositionApplicationSummaryAsync()
    {
        return await GetApplicationSummaryByPositionAsync();
    }

    public async Task<Result<IEnumerable<MonthlyApplicationSummaryDto>>> GetMonthlyApplicationSummaryAsync(int year)
    {
        try
        {
            var startDate = new DateTime(year, 1, 1);
            var endDate = new DateTime(year, 12, 31);
            var applications = await _unitOfWork.JobApplications.GetByDateRangeAsync(startDate, endDate);

            var monthlySummary = applications
                .GroupBy(a => new { a.ApplicationDate.Year, a.ApplicationDate.Month })
                .Select(g => new MonthlyApplicationSummaryDto
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    MonthName = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("MMMM"),
                    ApplicationCount = g.Count(),
                    ApprovedCount = g.Count(a => a.Status == JobApplicationStatus.Approved),
                    RejectedCount = g.Count(a => a.Status == JobApplicationStatus.Rejected),
                    ApprovalRate = g.Count() > 0 ? (decimal)g.Count(a => a.Status == JobApplicationStatus.Approved) / g.Count() * 100 : 0
                })
                .OrderBy(m => m.Month)
                .ToList();

            return Result<IEnumerable<MonthlyApplicationSummaryDto>>.Ok(monthlySummary);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating monthly application summary");
            return Result<IEnumerable<MonthlyApplicationSummaryDto>>.Fail($"Aylık istatistikler oluşturulamadı: {ex.Message}");
        }
    }

    public async Task<Result<int>> GetApplicationCountByStatusAsync(JobApplicationStatus status)
    {
        try
        {
            var count = await _unitOfWork.JobApplications.GetApplicationCountByStatusAsync(status);
            return Result<int>.Ok(count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting application count by status");
            return Result<int>.Fail($"Durum sayısı alınamadı: {ex.Message}");
        }
    }

    public async Task<Result<int>> GetApplicationCountByPositionAsync(int positionId)
    {
        try
        {
            var count = await _unitOfWork.JobApplications.GetApplicationCountByPositionAsync(positionId);
            return Result<int>.Ok(count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting application count by position");
            return Result<int>.Fail($"Pozisyon başvuru sayısı alınamadı: {ex.Message}");
        }
    }

    #endregion

    #region Export Operations

    public async Task<Result<byte[]>> ExportApplicationsAsync(JobApplicationFilterDto? filter = null)
    {
        try
        {
            IEnumerable<JobApplication> applications;
            
            if (filter != null)
            {
                applications = await _unitOfWork.JobApplications.GetFilteredAsync(
                    filter.Status, filter.PositionId, filter.StartDate, filter.EndDate, 
                    filter.SearchTerm, filter.ReviewedById);
            }
            else
            {
                applications = await _unitOfWork.JobApplications.GetAllAsync();
            }

            var dtos = _mapper.Map<IEnumerable<JobApplicationListDto>>(applications);
            var excelData = await _excelExportService.ExportAsync(dtos, "İş Başvuruları");
            
            return Result<byte[]>.Ok(excelData);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting applications");
            return Result<byte[]>.Fail($"Başvurular dışa aktarılamadı: {ex.Message}");
        }
    }

    public async Task<Result<byte[]>> ExportApplicationSummaryAsync()
    {
        try
        {
            var summary = await GetApplicationSummaryAsync();
            if (!summary.IsSuccess)
                return Result<byte[]>.Fail(summary.Message);

            var excelData = await _excelExportService.ExportAsync(new[] { summary.Data }, "Başvuru İstatistikleri");
            return Result<byte[]>.Ok(excelData);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting application summary");
            return Result<byte[]>.Fail($"İstatistikler dışa aktarılamadı: {ex.Message}");
        }
    }

    #endregion

    #region Bulk Operations

    public async Task<Result<bool>> BulkUpdateStatusAsync(IEnumerable<int> applicationIds, JobApplicationStatus status, int reviewedById)
    {
        try
        {
            var applications = new List<JobApplication>();
            foreach (var id in applicationIds)
            {
                var application = await _unitOfWork.JobApplications.GetByIdAsync(id);
                if (application != null)
                {
                    application.Status = status;
                    application.ReviewedById = reviewedById;
                    application.ReviewedAt = DateTime.Now;
                    application.UpdatedAt = DateTime.Now;
                    applications.Add(application);
                }
            }

            if (applications.Any())
            {
                foreach (var app in applications)
                {
                    _unitOfWork.JobApplications.Update(app);
                }
                await _unitOfWork.SaveChangesAsync();
            }

            _logger.LogInformation("Bulk status update completed for {Count} applications", applications.Count);
            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in bulk status update");
            return Result<bool>.Fail($"Toplu güncelleme yapılamadı: {ex.Message}");
        }
    }

    public async Task<Result<bool>> BulkDeleteAsync(IEnumerable<int> applicationIds)
    {
        try
        {
            var applications = new List<JobApplication>();
            foreach (var id in applicationIds)
            {
                var application = await _unitOfWork.JobApplications.GetByIdAsync(id);
                if (application != null)
                    applications.Add(application);
            }

            if (applications.Any())
            {
                foreach (var app in applications)
                {
                    _unitOfWork.JobApplications.Remove(app);
                }
                await _unitOfWork.SaveChangesAsync();
            }

            _logger.LogInformation("Bulk delete completed for {Count} applications", applications.Count);
            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in bulk delete");
            return Result<bool>.Fail($"Toplu silme yapılamadı: {ex.Message}");
        }
    }

    #endregion

    #region Email Integration (Future Implementation)

    public async Task<Result<bool>> SendApplicationConfirmationEmailAsync(int applicationId)
    {
        // Future implementation for email service
        return Result<bool>.Ok(true);
    }

    public async Task<Result<bool>> SendStatusUpdateEmailAsync(int applicationId)
    {
        // Future implementation for email service
        return Result<bool>.Ok(true);
    }

    public async Task<Result<bool>> SendInterviewInvitationEmailAsync(int applicationId)
    {
        // Future implementation for email service
        return Result<bool>.Ok(true);
    }

    #endregion

    #region Dashboard Support

    public async Task<Result<object>> GetDashboardStatisticsAsync()
    {
        try
        {
            var summary = await GetApplicationSummaryAsync();
            if (!summary.IsSuccess)
                return Result<object>.Fail(summary.Message);

            var recent = await GetRecentApplicationsAsync(5);
            var pending = await GetPendingApplicationsAsync();

            var dashboard = new
            {
                Summary = summary.Data,
                RecentApplications = recent.Data,
                PendingApplications = pending.Data?.Take(10),
                LastUpdated = DateTime.Now
            };

            return Result<object>.Ok(dashboard);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating dashboard statistics");
            return Result<object>.Fail($"Dashboard istatistikleri oluşturulamadı: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<JobApplicationListDto>>> GetApplicationsRequiringAttentionAsync()
    {
        try
        {
            var filter = new JobApplicationFilterDto
            {
                Status = JobApplicationStatus.Submitted
            };
            
            var result = await GetFilteredAsync(filter);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting applications requiring attention");
            return Result<IEnumerable<JobApplicationListDto>>.Fail($"Dikkat gerektiren başvurular getirilemedi: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<JobApplicationListDto>>> GetUpcomingInterviewsAsync(int daysAhead = 7)
    {
        try
        {
            var cutoffDate = DateTime.Now.AddDays(daysAhead);
            var applications = await _unitOfWork.JobApplications.GetAllAsync();
            
            var upcomingInterviews = applications
                .Where(a => a.Status == JobApplicationStatus.Interviewed && 
                           a.InterviewDate.HasValue && 
                           a.InterviewDate.Value >= DateTime.Now && 
                           a.InterviewDate.Value <= cutoffDate)
                .OrderBy(a => a.InterviewDate)
                .ToList();

            var dtos = _mapper.Map<IEnumerable<JobApplicationListDto>>(upcomingInterviews);
            return Result<IEnumerable<JobApplicationListDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting upcoming interviews");
            return Result<IEnumerable<JobApplicationListDto>>.Fail($"Yaklaşan mülakatlar getirilemedi: {ex.Message}");
        }
    }

    #endregion

    #region Private Helper Methods

    private async Task<Result<bool>> ValidateCreateAsync(JobApplicationCreateDto dto)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(dto.FirstName))
                return Result<bool>.Fail("Ad alanı gereklidir");

            if (string.IsNullOrWhiteSpace(dto.LastName))
                return Result<bool>.Fail("Soyad alanı gereklidir");

            if (string.IsNullOrWhiteSpace(dto.Email))
                return Result<bool>.Fail("E-posta alanı gereklidir");

            if (dto.PositionId <= 0)
                return Result<bool>.Fail("Geçerli bir pozisyon seçilmelidir");

            // Check if position exists and is available
            var position = await _unitOfWork.Positions.GetByIdAsync(dto.PositionId);
            if (position == null)
                return Result<bool>.Fail("Seçilen pozisyon bulunamadı");

            if (!position.IsActive)
                return Result<bool>.Fail("Seçilen pozisyon aktif değil");

            if (!position.IsAvailable)
                return Result<bool>.Fail("Seçilen pozisyon başvuruya açık değil");

            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating job application");
            return Result<bool>.Fail($"Başvuru doğrulanamadı: {ex.Message}");
        }
    }

    #endregion
}
