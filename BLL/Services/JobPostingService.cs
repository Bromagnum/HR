using AutoMapper;
using BLL.DTOs;
using BLL.Services.Export;
using BLL.Utilities;
using DAL.Entities;
using DAL.Repositories;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;

namespace BLL.Services;

public class JobPostingService : IJobPostingService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<JobPostingService> _logger;
    private readonly IExcelExportService _excelExportService;

    public JobPostingService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<JobPostingService> logger,
        IExcelExportService excelExportService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
        _excelExportService = excelExportService;
    }

    #region CRUD Operations

    public async Task<Result<IEnumerable<JobPostingListDto>>> GetAllAsync()
    {
        try
        {
            var postings = await _unitOfWork.JobPostings.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<JobPostingListDto>>(postings);
            return Result<IEnumerable<JobPostingListDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all job postings");
            return Result<IEnumerable<JobPostingListDto>>.Fail($"İş ilanları getirilemedi: {ex.Message}");
        }
    }

    public async Task<Result<JobPostingDetailDto?>> GetByIdAsync(int id)
    {
        try
        {
            if (id <= 0)
                return Result<JobPostingDetailDto?>.Fail("Geçersiz ilan ID'si");

            var posting = await _unitOfWork.JobPostings.GetByIdAsync(id);
            if (posting == null)
                return Result<JobPostingDetailDto?>.Fail("İlan bulunamadı");

            var dto = _mapper.Map<JobPostingDetailDto>(posting);
            return Result<JobPostingDetailDto?>.Ok(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving job posting with id {Id}", id);
            return Result<JobPostingDetailDto?>.Fail($"İlan getirilemedi: {ex.Message}");
        }
    }

    public async Task<Result<JobPostingDetailDto?>> GetBySlugAsync(string slug)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(slug))
                return Result<JobPostingDetailDto?>.Fail("Geçersiz slug");

            var posting = await _unitOfWork.JobPostings.GetBySlugAsync(slug);
            if (posting == null)
                return Result<JobPostingDetailDto?>.Fail("İlan bulunamadı");

            // Increment view count
            await IncrementViewCountAsync(posting.Id);

            var dto = _mapper.Map<JobPostingDetailDto>(posting);
            return Result<JobPostingDetailDto?>.Ok(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving job posting with slug {Slug}", slug);
            return Result<JobPostingDetailDto?>.Fail($"İlan getirilemedi: {ex.Message}");
        }
    }

    public async Task<Result<JobPostingDetailDto>> CreateAsync(JobPostingCreateDto dto)
    {
        try
        {
            // Validation
            var validationResult = await ValidateCreateAsync(dto);
            if (!validationResult.IsSuccess)
                return Result<JobPostingDetailDto>.Fail(validationResult.Message);

            // Generate slug if not provided
            if (string.IsNullOrWhiteSpace(dto.Slug))
            {
                var slugResult = await GenerateSlugAsync(dto.Title);
                if (slugResult.IsSuccess)
                    dto.Slug = slugResult.Data;
            }
            else
            {
                // Check if slug is unique
                var isUnique = await IsSlugUniqueAsync(dto.Slug);
                if (!isUnique.IsSuccess || !isUnique.Data)
                    return Result<JobPostingDetailDto>.Fail("Bu URL zaten kullanımda");
            }

            // Map and create
            var posting = _mapper.Map<JobPosting>(dto);
            posting.PublishDate = DateTime.Now;
            posting.Status = JobPostingStatus.Draft;

            await _unitOfWork.JobPostings.AddAsync(posting);
            await _unitOfWork.SaveChangesAsync();

            // Get with related data
            var result = await _unitOfWork.JobPostings.GetWithApplicationsAsync(posting.Id);
            var resultDto = _mapper.Map<JobPostingDetailDto>(result);

            _logger.LogInformation("Job posting created successfully with id {Id}", posting.Id);
            return Result<JobPostingDetailDto>.Ok(resultDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating job posting");
            return Result<JobPostingDetailDto>.Fail($"İlan oluşturulamadı: {ex.Message}");
        }
    }

    public async Task<Result<JobPostingDetailDto>> UpdateAsync(JobPostingUpdateDto dto)
    {
        try
        {
            if (dto.Id <= 0)
                return Result<JobPostingDetailDto>.Fail("Geçersiz ilan ID'si");

            var existingPosting = await _unitOfWork.JobPostings.GetByIdAsync(dto.Id);
            if (existingPosting == null)
                return Result<JobPostingDetailDto>.Fail("İlan bulunamadı");

            // Check slug uniqueness if changed
            if (!string.IsNullOrWhiteSpace(dto.Slug) && dto.Slug != existingPosting.Slug)
            {
                var isUnique = await IsSlugUniqueAsync(dto.Slug, dto.Id);
                if (!isUnique.IsSuccess || !isUnique.Data)
                    return Result<JobPostingDetailDto>.Fail("Bu URL zaten kullanımda");
            }

            // Map updates
            _mapper.Map(dto, existingPosting);
            existingPosting.UpdatedAt = DateTime.Now;

            _unitOfWork.JobPostings.Update(existingPosting);
            await _unitOfWork.SaveChangesAsync();

            var updatedPosting = await _unitOfWork.JobPostings.GetWithApplicationsAsync(dto.Id);
            var resultDto = _mapper.Map<JobPostingDetailDto>(updatedPosting);

            _logger.LogInformation("Job posting updated successfully with id {Id}", dto.Id);
            return Result<JobPostingDetailDto>.Ok(resultDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating job posting with id {Id}", dto.Id);
            return Result<JobPostingDetailDto>.Fail($"İlan güncellenemedi: {ex.Message}");
        }
    }

    public async Task<Result<bool>> DeleteAsync(int id)
    {
        try
        {
            if (id <= 0)
                return Result<bool>.Fail("Geçersiz ilan ID'si");

            var posting = await _unitOfWork.JobPostings.GetWithApplicationsAsync(id);
            if (posting == null)
                return Result<bool>.Fail("İlan bulunamadı");

            // Check if there are applications
            if (posting.Applications.Any())
                return Result<bool>.Fail("Başvurusu olan ilanlar silinemez");

            _unitOfWork.JobPostings.Remove(posting);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Job posting deleted successfully with id {Id}", id);
            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting job posting with id {Id}", id);
            return Result<bool>.Fail($"İlan silinemedi: {ex.Message}");
        }
    }

    public async Task<Result<bool>> SoftDeleteAsync(int id)
    {
        try
        {
            if (id <= 0)
                return Result<bool>.Fail("Geçersiz ilan ID'si");

            var posting = await _unitOfWork.JobPostings.GetByIdAsync(id);
            if (posting == null)
                return Result<bool>.Fail("İlan bulunamadı");

            // posting.IsActive = false; // IsActive is computed property, cannot set
            posting.Status = JobPostingStatus.Closed;
            posting.UpdatedAt = DateTime.Now;

            _unitOfWork.JobPostings.Update(posting);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Job posting soft deleted successfully with id {Id}", id);
            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error soft deleting job posting with id {Id}", id);
            return Result<bool>.Fail($"İlan pasif edilemedi: {ex.Message}");
        }
    }

    #endregion

    #region Public Job Listings

    public async Task<Result<IEnumerable<PublicJobPostingDto>>> GetActivePostingsAsync()
    {
        try
        {
            var postings = await _unitOfWork.JobPostings.GetActivePostingsAsync();
            var dtos = _mapper.Map<IEnumerable<PublicJobPostingDto>>(postings);
            return Result<IEnumerable<PublicJobPostingDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving active job postings");
            return Result<IEnumerable<PublicJobPostingDto>>.Fail($"Aktif ilanlar getirilemedi: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<PublicJobPostingDto>>> GetPublicListingsAsync(PublicJobPostingFilterDto? filter = null)
    {
        try
        {
            var postings = await _unitOfWork.JobPostings.GetActivePostingsAsync();
            var dtos = _mapper.Map<IEnumerable<PublicJobPostingDto>>(postings);
            return Result<IEnumerable<PublicJobPostingDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving public job listings");
            return Result<IEnumerable<PublicJobPostingDto>>.Fail($"İlanlar getirilemedi: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<PublicJobPostingDto>>> GetPublicPostingsAsync(JobPostingFilterDto? filter = null)
    {
        try
        {
            IEnumerable<JobPosting> postings;

            if (filter != null)
            {
                postings = await _unitOfWork.JobPostings.GetFilteredAsync(
                    JobPostingStatus.Active, // Only active postings for public
                    filter.DepartmentId,
                    filter.PositionId,
                    filter.EmploymentType,
                    filter.MinSalary,
                    filter.MaxSalary,
                    filter.Location,
                    filter.SearchTerm,
                    filter.IsRemoteWork);
            }
            else
            {
                postings = await _unitOfWork.JobPostings.GetActivePostingsAsync();
            }

            var dtos = _mapper.Map<IEnumerable<PublicJobPostingDto>>(postings);
            return Result<IEnumerable<PublicJobPostingDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving public job postings");
            return Result<IEnumerable<PublicJobPostingDto>>.Fail($"İlanlar getirilemedi: {ex.Message}");
        }
    }

    public async Task<Result<PublicJobPostingDto?>> GetPublicDetailsAsync(int id)
    {
        try
        {
            var posting = await _unitOfWork.JobPostings.GetByIdAsync(id);
            if (posting == null || posting.Status != JobPostingStatus.Active)
                return Result<PublicJobPostingDto?>.Ok(null);

            var dto = _mapper.Map<PublicJobPostingDto>(posting);
            return Result<PublicJobPostingDto?>.Ok(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving public job posting details for id {Id}", id);
            return Result<PublicJobPostingDto?>.Fail($"İlan detayları getirilemedi: {ex.Message}");
        }
    }

    public async Task<Result<PublicJobPostingDto?>> GetPublicPostingBySlugAsync(string slug)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(slug))
                return Result<PublicJobPostingDto?>.Fail("Geçersiz slug");

            var posting = await _unitOfWork.JobPostings.GetBySlugAsync(slug);
            if (posting == null || posting.Status != JobPostingStatus.Active)
                return Result<PublicJobPostingDto?>.Fail("İlan bulunamadı veya aktif değil");

            // Increment view count
            await IncrementViewCountAsync(posting.Id);

            var dto = _mapper.Map<PublicJobPostingDto>(posting);
            return Result<PublicJobPostingDto?>.Ok(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving public job posting with slug {Slug}", slug);
            return Result<PublicJobPostingDto?>.Fail($"İlan getirilemedi: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<PublicJobPostingDto>>> SearchPublicPostingsAsync(string searchTerm)
    {
        try
        {
            var postings = await _unitOfWork.JobPostings.SearchAsync(searchTerm);
            var activePostings = postings.Where(p => p.Status == JobPostingStatus.Active);
            var dtos = _mapper.Map<IEnumerable<PublicJobPostingDto>>(activePostings);
            return Result<IEnumerable<PublicJobPostingDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching public job postings with term {SearchTerm}", searchTerm);
            return Result<IEnumerable<PublicJobPostingDto>>.Fail($"İlan araması yapılamadı: {ex.Message}");
        }
    }

    #endregion

    #region Filtering & Search

    public async Task<Result<IEnumerable<JobPostingListDto>>> GetFilteredAsync(JobPostingFilterDto filter)
    {
        try
        {
            var postings = await _unitOfWork.JobPostings.GetFilteredAsync(
                filter.Status,
                filter.DepartmentId,
                filter.PositionId,
                filter.EmploymentType,
                filter.MinSalary,
                filter.MaxSalary,
                filter.Location,
                filter.SearchTerm,
                filter.IsRemoteWork);

            var dtos = _mapper.Map<IEnumerable<JobPostingListDto>>(postings);
            return Result<IEnumerable<JobPostingListDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error filtering job postings");
            return Result<IEnumerable<JobPostingListDto>>.Fail($"İlanlar filtrelenemiyor: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<JobPostingListDto>>> SearchAsync(string searchTerm)
    {
        try
        {
            var postings = await _unitOfWork.JobPostings.SearchAsync(searchTerm);
            var dtos = _mapper.Map<IEnumerable<JobPostingListDto>>(postings);
            return Result<IEnumerable<JobPostingListDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching job postings with term {SearchTerm}", searchTerm);
            return Result<IEnumerable<JobPostingListDto>>.Fail($"İlan araması yapılamadı: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<JobPostingListDto>>> GetByStatusAsync(JobPostingStatus status)
    {
        try
        {
            var postings = await _unitOfWork.JobPostings.GetByStatusAsync(status);
            var dtos = _mapper.Map<IEnumerable<JobPostingListDto>>(postings);
            return Result<IEnumerable<JobPostingListDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving job postings by status {Status}", status);
            return Result<IEnumerable<JobPostingListDto>>.Fail($"İlanlar getirilemedi: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<JobPostingListDto>>> GetByDepartmentIdAsync(int departmentId)
    {
        try
        {
            var postings = await _unitOfWork.JobPostings.GetByDepartmentIdAsync(departmentId);
            var dtos = _mapper.Map<IEnumerable<JobPostingListDto>>(postings);
            return Result<IEnumerable<JobPostingListDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving job postings by department {DepartmentId}", departmentId);
            return Result<IEnumerable<JobPostingListDto>>.Fail($"Departman ilanları getirilemedi: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<JobPostingListDto>>> GetByPositionIdAsync(int positionId)
    {
        try
        {
            var postings = await _unitOfWork.JobPostings.GetByPositionIdAsync(positionId);
            var dtos = _mapper.Map<IEnumerable<JobPostingListDto>>(postings);
            return Result<IEnumerable<JobPostingListDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving job postings by position {PositionId}", positionId);
            return Result<IEnumerable<JobPostingListDto>>.Fail($"Pozisyon ilanları getirilemedi: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<JobPostingListDto>>> GetExpiringPostingsAsync(int daysAhead = 7)
    {
        try
        {
            var postings = await _unitOfWork.JobPostings.GetExpiringPostingsAsync(daysAhead);
            var dtos = _mapper.Map<IEnumerable<JobPostingListDto>>(postings);
            return Result<IEnumerable<JobPostingListDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving expiring job postings");
            return Result<IEnumerable<JobPostingListDto>>.Fail($"Süresi dolan ilanlar getirilemedi: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<JobPostingListDto>>> GetRecentPostingsAsync(int count = 10)
    {
        try
        {
            var postings = await _unitOfWork.JobPostings.GetRecentPostingsAsync(count);
            var dtos = _mapper.Map<IEnumerable<JobPostingListDto>>(postings);
            return Result<IEnumerable<JobPostingListDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving recent job postings");
            return Result<IEnumerable<JobPostingListDto>>.Fail($"Son ilanlar getirilemedi: {ex.Message}");
        }
    }

    #endregion

    #region Status Management

    public async Task<Result<bool>> PublishAsync(int id, int publishedById)
    {
        return await UpdateStatusAsync(id, JobPostingStatus.Active, publishedById);
    }

    public async Task<Result<bool>> PublishPostingAsync(int id, int publishedById)
    {
        return await UpdateStatusAsync(id, JobPostingStatus.Active, publishedById);
    }

    public async Task<Result<bool>> SuspendAsync(int id, int suspendedById)
    {
        return await UpdateStatusAsync(id, JobPostingStatus.Suspended, suspendedById);
    }

    public async Task<Result<bool>> PausePostingAsync(int id, int pausedById)
    {
        return await UpdateStatusAsync(id, JobPostingStatus.Suspended, pausedById);
    }

    public async Task<Result<bool>> CloseAsync(int id, int closedById)
    {
        return await UpdateStatusAsync(id, JobPostingStatus.Closed, closedById);
    }

    public async Task<Result<bool>> ClosePostingAsync(int id, int closedById)
    {
        return await UpdateStatusAsync(id, JobPostingStatus.Closed, closedById);
    }

    public async Task<Result<bool>> MarkAsFilledAsync(int id, int filledById)
    {
        return await UpdateStatusAsync(id, JobPostingStatus.Closed, filledById);
    }

    public async Task<Result<bool>> UpdateStatusAsync(int id, JobPostingStatus status, int updatedById)
    {
        try
        {
            var posting = await _unitOfWork.JobPostings.GetByIdAsync(id);
            if (posting == null)
                return Result<bool>.Fail("İlan bulunamadı");

            posting.Status = status;
            posting.UpdatedById = updatedById;
            posting.UpdatedAt = DateTime.Now;

            // Set publish date if activating for the first time
            if (status == JobPostingStatus.Active && posting.PublishDate == default)
                posting.PublishDate = DateTime.Now;

            _unitOfWork.JobPostings.Update(posting);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Job posting status updated to {Status} for id {Id} by user {UpdatedById}", 
                status, id, updatedById);
            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating job posting status");
            return Result<bool>.Fail($"İlan durumu güncellenemedi: {ex.Message}");
        }
    }

    #endregion

    #region SEO & URL Management

    public async Task<Result<string>> GenerateSlugAsync(string title, int? excludeId = null)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(title))
                return Result<string>.Fail("Başlık boş olamaz");

            // Create base slug
            var baseSlug = CreateSlugFromTitle(title);
            var slug = baseSlug;
            var counter = 1;

            // Check uniqueness and add counter if needed
            while (true)
            {
                var isUnique = await IsSlugUniqueAsync(slug, excludeId);
                if (isUnique.IsSuccess && isUnique.Data)
                    break;

                slug = $"{baseSlug}-{counter}";
                counter++;

                if (counter > 100) // Safety check
                    return Result<string>.Fail("Benzersiz URL oluşturulamadı");
            }

            return Result<string>.Ok(slug);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating slug for title {Title}", title);
            return Result<string>.Fail($"URL oluşturulamadı: {ex.Message}");
        }
    }

    public async Task<Result<bool>> IsSlugUniqueAsync(string slug, int? excludeId = null)
    {
        try
        {
            var isUnique = await _unitOfWork.JobPostings.IsSlugUniqueAsync(slug, excludeId);
            return Result<bool>.Ok(isUnique);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking slug uniqueness");
            return Result<bool>.Fail($"URL benzersizliği kontrol edilemedi: {ex.Message}");
        }
    }

    public async Task<Result<bool>> UpdateSlugAsync(int id, string slug)
    {
        try
        {
            var posting = await _unitOfWork.JobPostings.GetByIdAsync(id);
            if (posting == null)
                return Result<bool>.Fail("İlan bulunamadı");

            var isUnique = await IsSlugUniqueAsync(slug, id);
            if (!isUnique.IsSuccess || !isUnique.Data)
                return Result<bool>.Fail("Bu URL zaten kullanımda");

            posting.Slug = slug;
            posting.UpdatedAt = DateTime.Now;

            _unitOfWork.JobPostings.Update(posting);
            await _unitOfWork.SaveChangesAsync();

            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating slug for posting {Id}", id);
            return Result<bool>.Fail($"URL güncellenemedi: {ex.Message}");
        }
    }

    #endregion

    #region View & Application Tracking

    public async Task<Result<bool>> IncrementViewCountAsync(int id)
    {
        try
        {
            await _unitOfWork.JobPostings.IncrementViewCountAsync(id);
            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error incrementing view count for posting {Id}", id);
            return Result<bool>.Fail($"Görüntüleme sayısı güncellenemedi: {ex.Message}");
        }
    }

    public async Task<Result<bool>> UpdateApplicationCountAsync(int id)
    {
        try
        {
            await _unitOfWork.JobPostings.UpdateApplicationCountAsync(id);
            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating application count for posting {Id}", id);
            return Result<bool>.Fail($"Başvuru sayısı güncellenemedi: {ex.Message}");
        }
    }

    public async Task<Result<JobPostingDetailDto?>> GetWithApplicationsAsync(int id)
    {
        try
        {
            var posting = await _unitOfWork.JobPostings.GetWithApplicationsAsync(id);
            if (posting == null)
                return Result<JobPostingDetailDto?>.Fail("İlan bulunamadı");

            var dto = _mapper.Map<JobPostingDetailDto>(posting);
            return Result<JobPostingDetailDto?>.Ok(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving job posting with applications");
            return Result<JobPostingDetailDto?>.Fail($"İlan ve başvurular getirilemedi: {ex.Message}");
        }
    }

    #endregion

    #region Validation & Business Rules

    public async Task<Result<bool>> CanPublishPostingAsync(int id)
    {
        try
        {
            var posting = await _unitOfWork.JobPostings.GetByIdAsync(id);
            if (posting == null)
                return Result<bool>.Fail("İlan bulunamadı");

            if (string.IsNullOrWhiteSpace(posting.Title))
                return Result<bool>.Fail("İlan başlığı gerekli");

            if (string.IsNullOrWhiteSpace(posting.Description))
                return Result<bool>.Fail("İş tanımı gerekli");

            if (posting.PositionId <= 0)
                return Result<bool>.Fail("Pozisyon seçimi gerekli");

            if (posting.DepartmentId <= 0)
                return Result<bool>.Fail("Departman seçimi gerekli");

            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if posting can be published");
            return Result<bool>.Fail($"Yayınlama kontrolü yapılamadı: {ex.Message}");
        }
    }

    public async Task<Result<bool>> IsPostingActiveAsync(int id)
    {
        try
        {
            var posting = await _unitOfWork.JobPostings.GetByIdAsync(id);
            if (posting == null)
                return Result<bool>.Ok(false);

            var isActive = posting.Status == JobPostingStatus.Active && 
                          posting.IsActive && 
                          (!posting.ExpiryDate.HasValue || posting.ExpiryDate.Value > DateTime.Now) &&
                          (!posting.LastApplicationDate.HasValue || posting.LastApplicationDate.Value > DateTime.Now);

            return Result<bool>.Ok(isActive);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if posting is active");
            return Result<bool>.Fail($"Aktiflik kontrolü yapılamadı: {ex.Message}");
        }
    }

    public async Task<Result<bool>> IsApplicationDeadlinePassedAsync(int id)
    {
        try
        {
            var posting = await _unitOfWork.JobPostings.GetByIdAsync(id);
            if (posting == null)
                return Result<bool>.Fail("İlan bulunamadı");

            var isPassed = posting.LastApplicationDate.HasValue && 
                          posting.LastApplicationDate.Value < DateTime.Now;

            return Result<bool>.Ok(isPassed);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking application deadline");
            return Result<bool>.Fail($"Başvuru tarihi kontrolü yapılamadı: {ex.Message}");
        }
    }

    public async Task<Result<bool>> HasActivePostingForPositionAsync(int positionId)
    {
        try
        {
            var postings = await _unitOfWork.JobPostings.GetByPositionIdAsync(positionId);
            var hasActive = postings.Any(p => p.Status == JobPostingStatus.Active && p.IsActive);
            return Result<bool>.Ok(hasActive);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking active posting for position");
            return Result<bool>.Fail($"Aktif ilan kontrolü yapılamadı: {ex.Message}");
        }
    }

    #endregion

    #region Statistics & Reports

    public async Task<Result<JobPostingSummaryDto>> GetPostingSummaryAsync()
    {
        try
        {
            var postings = await _unitOfWork.JobPostings.GetAllAsync();
            var summary = new JobPostingSummaryDto
            {
                TotalPostings = postings.Count(),
                ActivePostings = postings.Count(p => p.Status == JobPostingStatus.Active),
                DraftPostings = postings.Count(p => p.Status == JobPostingStatus.Draft),
                PausedPostings = postings.Count(p => p.Status == JobPostingStatus.Suspended),
                ClosedPostings = postings.Count(p => p.Status == JobPostingStatus.Closed),
                FilledPostings = postings.Count(p => p.Status == JobPostingStatus.Closed),
                ExpiringPostings = postings.Count(p => p.ExpiryDate.HasValue && 
                                                      p.ExpiryDate.Value <= DateTime.Now.AddDays(7) && 
                                                      p.ExpiryDate.Value > DateTime.Now),
                TotalViews = postings.Sum(p => p.ViewCount),
                TotalApplications = postings.Sum(p => p.ApplicationCount)
            };

            return Result<JobPostingSummaryDto>.Ok(summary);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating posting summary");
            return Result<JobPostingSummaryDto>.Fail($"İstatistik oluşturulamadı: {ex.Message}");
        }
    }

    public async Task<Result<JobPostingSummaryDto>> GetSummaryAsync()
    {
        return await GetPostingSummaryAsync();
    }

    public async Task<Result<JobPostingSummaryDto>> GetPostingSummaryByPeriodAsync(DateTime startDate, DateTime endDate)
    {
        try
        {
            var postings = await _unitOfWork.JobPostings.GetAllAsync();
            var filteredPostings = postings.Where(p => p.PublishDate >= startDate && p.PublishDate <= endDate).ToList();

            var summary = new JobPostingSummaryDto
            {
                TotalPostings = filteredPostings.Count,
                ActivePostings = filteredPostings.Count(p => p.Status == JobPostingStatus.Active),
                DraftPostings = filteredPostings.Count(p => p.Status == JobPostingStatus.Draft),
                PausedPostings = filteredPostings.Count(p => p.Status == JobPostingStatus.Suspended),
                ClosedPostings = filteredPostings.Count(p => p.Status == JobPostingStatus.Closed),
                FilledPostings = filteredPostings.Count(p => p.Status == JobPostingStatus.Closed),
                TotalViews = filteredPostings.Sum(p => p.ViewCount),
                TotalApplications = filteredPostings.Sum(p => p.ApplicationCount)
            };

            return Result<JobPostingSummaryDto>.Ok(summary);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating posting summary by period");
            return Result<JobPostingSummaryDto>.Fail($"Dönemsel istatistik oluşturulamadı: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<DepartmentPostingSummaryDto>>> GetPostingSummaryByDepartmentAsync()
    {
        try
        {
            var postings = await _unitOfWork.JobPostings.GetAllAsync();
            var departmentSummary = postings
                .GroupBy(p => p.Department)
                .Select(g => new DepartmentPostingSummaryDto
                {
                    DepartmentId = g.Key.Id,
                    DepartmentName = g.Key.Name,
                    PostingCount = g.Count(),
                    ActivePostingCount = g.Count(p => p.Status == JobPostingStatus.Active),
                    TotalApplications = g.Sum(p => p.ApplicationCount),
                    AverageApplicationsPerPosting = g.Count() > 0 ? (decimal)g.Sum(p => p.ApplicationCount) / g.Count() : 0
                })
                .ToList();

            return Result<IEnumerable<DepartmentPostingSummaryDto>>.Ok(departmentSummary);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating posting summary by department");
            return Result<IEnumerable<DepartmentPostingSummaryDto>>.Fail($"Departman istatistikleri oluşturulamadı: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<DepartmentPostingSummaryDto>>> GetDepartmentPostingSummaryAsync()
    {
        return await GetPostingSummaryByDepartmentAsync();
    }

    public async Task<Result<IEnumerable<MonthlyPostingSummaryDto>>> GetMonthlyPostingSummaryAsync(int year)
    {
        try
        {
            var startDate = new DateTime(year, 1, 1);
            var endDate = new DateTime(year, 12, 31);
            var postings = await _unitOfWork.JobPostings.GetAllAsync();
            var yearPostings = postings.Where(p => p.PublishDate >= startDate && p.PublishDate <= endDate).ToList();

            var monthlySummary = yearPostings
                .GroupBy(p => new { p.PublishDate.Year, p.PublishDate.Month })
                .Select(g => new MonthlyPostingSummaryDto
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    MonthName = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("MMMM"),
                    PostingCount = g.Count(),
                    ApplicationCount = g.Sum(p => p.ApplicationCount),
                    TotalViews = g.Sum(p => p.ViewCount),
                    AverageApplicationsPerPosting = g.Count() > 0 ? (decimal)g.Sum(p => p.ApplicationCount) / g.Count() : 0
                })
                .OrderBy(m => m.Month)
                .ToList();

            return Result<IEnumerable<MonthlyPostingSummaryDto>>.Ok(monthlySummary);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating monthly posting summary");
            return Result<IEnumerable<MonthlyPostingSummaryDto>>.Fail($"Aylık istatistikler oluşturulamadı: {ex.Message}");
        }
    }

    public async Task<Result<int>> GetPostingCountByStatusAsync(JobPostingStatus status)
    {
        try
        {
            var postings = await _unitOfWork.JobPostings.GetByStatusAsync(status);
            return Result<int>.Ok(postings.Count());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting posting count by status");
            return Result<int>.Fail($"Durum sayısı alınamadı: {ex.Message}");
        }
    }

    public async Task<Result<int>> GetPostingCountByDepartmentAsync(int departmentId)
    {
        try
        {
            var postings = await _unitOfWork.JobPostings.GetByDepartmentIdAsync(departmentId);
            return Result<int>.Ok(postings.Count());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting posting count by department");
            return Result<int>.Fail($"Departman ilan sayısı alınamadı: {ex.Message}");
        }
    }

    #endregion

    #region Performance Analytics

    public async Task<Result<object>> GetPostingPerformanceAsync(int id)
    {
        try
        {
            var posting = await _unitOfWork.JobPostings.GetWithApplicationsAsync(id);
            if (posting == null)
                return Result<object>.Fail("İlan bulunamadı");

            var performance = new
            {
                PostingId = posting.Id,
                Title = posting.Title,
                ViewCount = posting.ViewCount,
                ApplicationCount = posting.ApplicationCount,
                ConversionRate = posting.ViewCount > 0 ? (decimal)posting.ApplicationCount / posting.ViewCount * 100 : 0,
                DaysActive = posting.Status == JobPostingStatus.Active ? (DateTime.Now - posting.PublishDate).Days : 0,
                AverageApplicationsPerDay = posting.Status == JobPostingStatus.Active && (DateTime.Now - posting.PublishDate).Days > 0 
                    ? (decimal)posting.ApplicationCount / (DateTime.Now - posting.PublishDate).Days : 0
            };

            return Result<object>.Ok(performance);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting posting performance");
            return Result<object>.Fail($"Performans analizi yapılamadı: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<object>>> GetTopPerformingPostingsAsync(int count = 10)
    {
        try
        {
            var postings = await _unitOfWork.JobPostings.GetAllAsync();
            var topPerforming = postings
                .Where(p => p.ViewCount > 0)
                .OrderByDescending(p => (decimal)p.ApplicationCount / p.ViewCount)
                .Take(count)
                .Select(p => new
                {
                    PostingId = p.Id,
                    Title = p.Title,
                    ViewCount = p.ViewCount,
                    ApplicationCount = p.ApplicationCount,
                    ConversionRate = (decimal)p.ApplicationCount / p.ViewCount * 100
                })
                .ToList();

            return Result<IEnumerable<object>>.Ok(topPerforming);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting top performing postings");
            return Result<IEnumerable<object>>.Fail($"En iyi performans analizi yapılamadı: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<object>>> GetPoorPerformingPostingsAsync(int count = 10)
    {
        try
        {
            var postings = await _unitOfWork.JobPostings.GetAllAsync();
            var poorPerforming = postings
                .Where(p => p.ViewCount > 10) // At least 10 views to be considered
                .OrderBy(p => (decimal)p.ApplicationCount / p.ViewCount)
                .Take(count)
                .Select(p => new
                {
                    PostingId = p.Id,
                    Title = p.Title,
                    ViewCount = p.ViewCount,
                    ApplicationCount = p.ApplicationCount,
                    ConversionRate = (decimal)p.ApplicationCount / p.ViewCount * 100
                })
                .ToList();

            return Result<IEnumerable<object>>.Ok(poorPerforming);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting poor performing postings");
            return Result<IEnumerable<object>>.Fail($"Düşük performans analizi yapılamadı: {ex.Message}");
        }
    }

    #endregion

    #region Export Operations

    public async Task<Result<byte[]>> ExportPostingsAsync(JobPostingFilterDto? filter = null)
    {
        try
        {
            IEnumerable<JobPosting> postings;
            
            if (filter != null)
            {
                postings = await _unitOfWork.JobPostings.GetFilteredAsync(
                    filter.Status, filter.DepartmentId, filter.PositionId, filter.EmploymentType,
                    filter.MinSalary, filter.MaxSalary, filter.Location, filter.SearchTerm, filter.IsRemoteWork);
            }
            else
            {
                postings = await _unitOfWork.JobPostings.GetAllAsync();
            }

            var dtos = _mapper.Map<IEnumerable<JobPostingListDto>>(postings);
            var excelData = await _excelExportService.ExportAsync(dtos, "İş İlanları");
            
            return Result<byte[]>.Ok(excelData);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting postings");
            return Result<byte[]>.Fail($"İlanlar dışa aktarılamadı: {ex.Message}");
        }
    }

    public async Task<Result<byte[]>> ExportPostingSummaryAsync()
    {
        try
        {
            var summary = await GetPostingSummaryAsync();
            if (!summary.IsSuccess)
                return Result<byte[]>.Fail(summary.Message);

            var excelData = await _excelExportService.ExportAsync(new[] { summary.Data }, "İlan İstatistikleri");
            return Result<byte[]>.Ok(excelData);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting posting summary");
            return Result<byte[]>.Fail($"İstatistikler dışa aktarılamadı: {ex.Message}");
        }
    }

    public async Task<Result<byte[]>> ExportApplicationsByPostingAsync(int postingId)
    {
        try
        {
            var posting = await _unitOfWork.JobPostings.GetWithApplicationsAsync(postingId);
            if (posting == null)
                return Result<byte[]>.Fail("İlan bulunamadı");

            var applicationDtos = _mapper.Map<IEnumerable<JobApplicationListDto>>(posting.Applications);
            var excelData = await _excelExportService.ExportAsync(applicationDtos, $"{posting.Title} Başvuruları");
            
            return Result<byte[]>.Ok(excelData);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting applications by posting");
            return Result<byte[]>.Fail($"İlan başvuruları dışa aktarılamadı: {ex.Message}");
        }
    }

    #endregion

    #region Bulk Operations

    public async Task<Result<bool>> BulkUpdateStatusAsync(IEnumerable<int> postingIds, JobPostingStatus status, int updatedById)
    {
        try
        {
            var postings = new List<JobPosting>();
            foreach (var id in postingIds)
            {
                var posting = await _unitOfWork.JobPostings.GetByIdAsync(id);
                if (posting != null)
                {
                    posting.Status = status;
                    posting.UpdatedById = updatedById;
                    posting.UpdatedAt = DateTime.Now;
                    postings.Add(posting);
                }
            }

            if (postings.Any())
            {
                foreach (var posting in postings)
                {
                    _unitOfWork.JobPostings.Update(posting);
                }
                await _unitOfWork.SaveChangesAsync();
            }

            _logger.LogInformation("Bulk status update completed for {Count} postings", postings.Count);
            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in bulk status update");
            return Result<bool>.Fail($"Toplu güncelleme yapılamadı: {ex.Message}");
        }
    }

    public async Task<Result<bool>> BulkDeleteAsync(IEnumerable<int> postingIds)
    {
        try
        {
            var postings = new List<JobPosting>();
            foreach (var id in postingIds)
            {
                var posting = await _unitOfWork.JobPostings.GetWithApplicationsAsync(id);
                if (posting != null)
                {
                    if (posting.Applications.Any())
                    {
                        _logger.LogWarning("Cannot delete posting {Id} because it has applications", id);
                        continue;
                    }
                    postings.Add(posting);
                }
            }

            if (postings.Any())
            {
                foreach (var posting in postings)
                {
                    _unitOfWork.JobPostings.Remove(posting);
                }
                await _unitOfWork.SaveChangesAsync();
            }

            _logger.LogInformation("Bulk delete completed for {Count} postings", postings.Count);
            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in bulk delete");
            return Result<bool>.Fail($"Toplu silme yapılamadı: {ex.Message}");
        }
    }

    public async Task<Result<bool>> BulkExtendExpiryDateAsync(IEnumerable<int> postingIds, DateTime newExpiryDate)
    {
        try
        {
            var postings = new List<JobPosting>();
            foreach (var id in postingIds)
            {
                var posting = await _unitOfWork.JobPostings.GetByIdAsync(id);
                if (posting != null)
                {
                    posting.ExpiryDate = newExpiryDate;
                    posting.UpdatedAt = DateTime.Now;
                    postings.Add(posting);
                }
            }

            if (postings.Any())
            {
                foreach (var posting in postings)
                {
                    _unitOfWork.JobPostings.Update(posting);
                }
                await _unitOfWork.SaveChangesAsync();
            }

            _logger.LogInformation("Bulk expiry date update completed for {Count} postings", postings.Count);
            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in bulk expiry date update");
            return Result<bool>.Fail($"Toplu süre uzatma yapılamadı: {ex.Message}");
        }
    }

    #endregion

    #region Template & Cloning

    public async Task<Result<JobPostingCreateDto>> ClonePostingAsync(int id)
    {
        try
        {
            var posting = await _unitOfWork.JobPostings.GetByIdAsync(id);
            if (posting == null)
                return Result<JobPostingCreateDto>.Fail("İlan bulunamadı");

            var cloneDto = new JobPostingCreateDto
            {
                Title = $"{posting.Title} (Kopya)",
                Description = posting.Description,
                PositionId = posting.PositionId,
                DepartmentId = posting.DepartmentId,
                EmploymentType = posting.EmploymentType,
                Requirements = posting.Requirements,
                Responsibilities = posting.Responsibilities,
                Benefits = posting.Benefits,
                MinExperience = posting.MinExperience,
                MaxExperience = posting.MaxExperience,
                MinEducation = posting.MinEducation,
                MinSalary = posting.MinSalary,
                MaxSalary = posting.MaxSalary,
                Location = posting.Location,
                IsRemoteWork = posting.IsRemoteWork,
                OpenPositions = posting.OpenPositions,
                ContactInfo = posting.ContactInfo,
                Tags = posting.Tags
            };

            return Result<JobPostingCreateDto>.Ok(cloneDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cloning posting {Id}", id);
            return Result<JobPostingCreateDto>.Fail($"İlan kopyalanamadı: {ex.Message}");
        }
    }

    public async Task<Result<JobPostingCreateDto>> CreateFromTemplateAsync(int templateId)
    {
        // This would be implemented when template system is available
        // For now, just clone the posting
        return await ClonePostingAsync(templateId);
    }

    #endregion

    #region Notification & Alerts

    public async Task<Result<bool>> SendExpiryWarningAsync(int id)
    {
        // Future implementation for notification service
        return Result<bool>.Ok(true);
    }

    public async Task<Result<IEnumerable<JobPostingListDto>>> GetPostingsRequiringAttentionAsync()
    {
        try
        {
            var expiring = await GetExpiringPostingsAsync(7);
            return expiring;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting postings requiring attention");
            return Result<IEnumerable<JobPostingListDto>>.Fail($"Dikkat gerektiren ilanlar getirilemedi: {ex.Message}");
        }
    }

    #endregion

    #region Dashboard Support

    public async Task<Result<object>> GetDashboardStatisticsAsync()
    {
        try
        {
            var summary = await GetPostingSummaryAsync();
            if (!summary.IsSuccess)
                return Result<object>.Fail(summary.Message);

            var recent = await GetRecentPostingsAsync(5);
            var expiring = await GetExpiringPostingsAsync(7);

            var dashboard = new
            {
                Summary = summary.Data,
                RecentPostings = recent.Data,
                ExpiringPostings = expiring.Data,
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

    public async Task<Result<IEnumerable<JobPostingListDto>>> GetTrendingPostingsAsync(int count = 5)
    {
        try
        {
            var postings = await _unitOfWork.JobPostings.GetAllAsync();
            var trending = postings
                .Where(p => p.Status == JobPostingStatus.Active)
                .OrderByDescending(p => p.ViewCount)
                .ThenByDescending(p => p.ApplicationCount)
                .Take(count)
                .ToList();

            var dtos = _mapper.Map<IEnumerable<JobPostingListDto>>(trending);
            return Result<IEnumerable<JobPostingListDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting trending postings");
            return Result<IEnumerable<JobPostingListDto>>.Fail($"Trend ilanlar getirilemedi: {ex.Message}");
        }
    }

    #endregion

    #region Integration Support

    public async Task<Result<string>> GenerateJobPostingXmlAsync(int id)
    {
        try
        {
            var posting = await _unitOfWork.JobPostings.GetByIdAsync(id);
            if (posting == null)
                return Result<string>.Fail("İlan bulunamadı");

            var xml = new XElement("JobPosting",
                new XElement("Id", posting.Id),
                new XElement("Title", posting.Title),
                new XElement("Description", posting.Description),
                new XElement("Position", posting.Position?.Name),
                new XElement("Department", posting.Department?.Name),
                new XElement("Location", posting.Location),
                new XElement("EmploymentType", posting.EmploymentType.ToString()),
                new XElement("SalaryRange", posting.SalaryRange),
                new XElement("PublishDate", posting.PublishDate.ToString("yyyy-MM-dd")),
                new XElement("ExpiryDate", posting.ExpiryDate?.ToString("yyyy-MM-dd")),
                new XElement("IsRemoteWork", posting.IsRemoteWork),
                new XElement("Requirements", posting.Requirements),
                new XElement("Responsibilities", posting.Responsibilities),
                new XElement("Benefits", posting.Benefits)
            );

            return Result<string>.Ok(xml.ToString());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating XML for posting {Id}", id);
            return Result<string>.Fail($"XML oluşturulamadı: {ex.Message}");
        }
    }

    public async Task<Result<string>> GenerateJobPostingJsonAsync(int id)
    {
        try
        {
            var posting = await _unitOfWork.JobPostings.GetByIdAsync(id);
            if (posting == null)
                return Result<string>.Fail("İlan bulunamadı");

            var publicDto = _mapper.Map<PublicJobPostingDto>(posting);
            var json = JsonSerializer.Serialize(publicDto, new JsonSerializerOptions 
            { 
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            return Result<string>.Ok(json);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating JSON for posting {Id}", id);
            return Result<string>.Fail($"JSON oluşturulamadı: {ex.Message}");
        }
    }

    public async Task<Result<bool>> SyncWithExternalJobBoardsAsync(int id)
    {
        // Future implementation for external job board integration
        return Result<bool>.Ok(true);
    }

    #endregion

    #region Private Helper Methods

    private async Task<Result<bool>> ValidateCreateAsync(JobPostingCreateDto dto)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(dto.Title))
                return Result<bool>.Fail("İlan başlığı gereklidir");

            if (string.IsNullOrWhiteSpace(dto.Description))
                return Result<bool>.Fail("İş tanımı gereklidir");

            if (dto.PositionId <= 0)
                return Result<bool>.Fail("Geçerli bir pozisyon seçilmelidir");

            if (dto.DepartmentId <= 0)
                return Result<bool>.Fail("Geçerli bir departman seçilmelidir");

            // Check if position exists
            var position = await _unitOfWork.Positions.GetByIdAsync(dto.PositionId);
            if (position == null)
                return Result<bool>.Fail("Seçilen pozisyon bulunamadı");

            // Check if department exists
            var department = await _unitOfWork.Departments.GetByIdAsync(dto.DepartmentId);
            if (department == null)
                return Result<bool>.Fail("Seçilen departman bulunamadı");

            // Validate salary range
            if (dto.MinSalary.HasValue && dto.MaxSalary.HasValue && dto.MinSalary > dto.MaxSalary)
                return Result<bool>.Fail("Minimum maaş maksimum maaştan büyük olamaz");

            // Validate experience range
            if (dto.MinExperience.HasValue && dto.MaxExperience.HasValue && dto.MinExperience > dto.MaxExperience)
                return Result<bool>.Fail("Minimum deneyim maksimum deneyimden büyük olamaz");

            // Validate dates
            if (dto.ExpiryDate.HasValue && dto.ExpiryDate.Value <= DateTime.Now)
                return Result<bool>.Fail("Son başvuru tarihi gelecekte olmalıdır");

            if (dto.LastApplicationDate.HasValue && dto.LastApplicationDate.Value <= DateTime.Now)
                return Result<bool>.Fail("Son başvuru tarihi gelecekte olmalıdır");

            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating job posting");
            return Result<bool>.Fail($"İlan doğrulanamadı: {ex.Message}");
        }
    }

    private static string CreateSlugFromTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            return "job-posting";

        // Convert to lowercase and replace Turkish characters
        var slug = title.ToLowerInvariant()
            .Replace('ç', 'c').Replace('ğ', 'g').Replace('ı', 'i')
            .Replace('ö', 'o').Replace('ş', 's').Replace('ü', 'u');

        // Remove special characters and replace spaces with hyphens
        var sb = new StringBuilder();
        foreach (var c in slug)
        {
            if (char.IsLetterOrDigit(c))
                sb.Append(c);
            else if (char.IsWhiteSpace(c) && sb.Length > 0 && sb[sb.Length - 1] != '-')
                sb.Append('-');
        }

        // Remove trailing hyphens and limit length
        var result = sb.ToString().Trim('-');
        return result.Length > 100 ? result.Substring(0, 100).TrimEnd('-') : result;
    }

    #endregion
}
