using AutoMapper;
using BLL.DTOs;
using BLL.Utilities;
using DAL.Entities;
using DAL.Repositories;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace BLL.Services;

public class PerformanceReviewService : IPerformanceReviewService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<PerformanceReviewService> _logger;

    public PerformanceReviewService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<PerformanceReviewService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    #region Performance Review Operations

    public async Task<Result<IEnumerable<PerformanceReviewListDto>>> GetAllAsync()
    {
        try
        {
            var reviews = await _unitOfWork.PerformanceReviews.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<PerformanceReviewListDto>>(reviews);
            return Result<IEnumerable<PerformanceReviewListDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all performance reviews");
            return Result<IEnumerable<PerformanceReviewListDto>>.Fail("Performans değerlendirmeleri getirilirken hata oluştu");
        }
    }

    public async Task<Result<PerformanceReviewDetailDto?>> GetByIdAsync(int id)
    {
        try
        {
            var review = await _unitOfWork.PerformanceReviews.GetByIdAsync(id);
            if (review == null)
                return Result<PerformanceReviewDetailDto?>.Fail("Performans değerlendirmesi bulunamadı");

            var dto = _mapper.Map<PerformanceReviewDetailDto>(review);
            return Result<PerformanceReviewDetailDto?>.Ok(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting performance review by id: {Id}", id);
            return Result<PerformanceReviewDetailDto?>.Fail("Performans değerlendirmesi getirilirken hata oluştu");
        }
    }

    public async Task<Result<PerformanceReviewDetailDto>> CreateAsync(PerformanceReviewCreateDto dto)
    {
        try
        {
            // Validation
            var canCreateResult = await CanCreateReviewAsync(dto.PersonId, dto.ReviewPeriodId);
            if (!canCreateResult.IsSuccess || !canCreateResult.Data)
                return Result<PerformanceReviewDetailDto>.Fail("Bu personel için bu dönemde zaten bir değerlendirme mevcut");

            var review = _mapper.Map<PerformanceReview>(dto);
            review.Status = ReviewStatus.Draft;
            review.CreatedAt = DateTime.Now;

            await _unitOfWork.PerformanceReviews.AddAsync(review);
            await _unitOfWork.SaveChangesAsync();

            // Add goals if provided
            if (dto.Goals_List.Any())
            {
                foreach (var goalDto in dto.Goals_List)
                {
                    var goal = _mapper.Map<PerformanceGoal>(goalDto);
                    goal.PerformanceReviewId = review.Id;
                    goal.Status = GoalStatus.NotStarted;
                    goal.CreatedAt = DateTime.Now;
                    await _unitOfWork.PerformanceGoals.AddAsync(goal);
                }
                await _unitOfWork.SaveChangesAsync();
            }

            var result = await GetByIdAsync(review.Id);
            _logger.LogInformation("Performance review created for person {PersonId} in period {PeriodId}", dto.PersonId, dto.ReviewPeriodId);
            return Result<PerformanceReviewDetailDto>.Ok(result.Data!);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating performance review");
            return Result<PerformanceReviewDetailDto>.Fail("Performans değerlendirmesi oluşturulurken hata oluştu");
        }
    }

    public async Task<Result<PerformanceReviewDetailDto>> UpdateAsync(PerformanceReviewUpdateDto dto)
    {
        try
        {
            var review = await _unitOfWork.PerformanceReviews.GetByIdAsync(dto.Id);
            if (review == null)
                return Result<PerformanceReviewDetailDto>.Fail("Performans değerlendirmesi bulunamadı");

            // Check if review can be edited
            if (review.Status == ReviewStatus.Approved || review.Status == ReviewStatus.Completed)
                return Result<PerformanceReviewDetailDto>.Fail("Onaylanmış veya tamamlanmış değerlendirme düzenlenemez");

            _mapper.Map(dto, review);
            review.UpdatedAt = DateTime.Now;

            _unitOfWork.PerformanceReviews.Update(review);
            await _unitOfWork.SaveChangesAsync();

            var result = await GetByIdAsync(dto.Id);
            _logger.LogInformation("Performance review updated: {ReviewId}", dto.Id);
            return Result<PerformanceReviewDetailDto>.Ok(result.Data!);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating performance review: {ReviewId}", dto.Id);
            return Result<PerformanceReviewDetailDto>.Fail("Performans değerlendirmesi güncellenirken hata oluştu");
        }
    }

    public async Task<Result> DeleteAsync(int id)
    {
        try
        {
            var review = await _unitOfWork.PerformanceReviews.GetByIdAsync(id);
            if (review == null)
                return Result.Fail("Performans değerlendirmesi bulunamadı");

            if (review.Status != ReviewStatus.Draft)
                return Result.Fail("Sadece taslak durumundaki değerlendirmeler silinebilir");

            _unitOfWork.PerformanceReviews.Remove(review);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Performance review deleted: {ReviewId}", id);
            return Result.Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting performance review: {ReviewId}", id);
            return Result.Fail("Performans değerlendirmesi silinirken hata oluştu");
        }
    }

    #endregion

    #region Review Management

    public async Task<Result<IEnumerable<PerformanceReviewListDto>>> GetByPersonIdAsync(int personId)
    {
        try
        {
            var reviews = await _unitOfWork.PerformanceReviews.GetByPersonIdAsync(personId);
            var dtos = _mapper.Map<IEnumerable<PerformanceReviewListDto>>(reviews);
            return Result<IEnumerable<PerformanceReviewListDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting performance reviews for person: {PersonId}", personId);
            return Result<IEnumerable<PerformanceReviewListDto>>.Fail("Personel değerlendirmeleri getirilirken hata oluştu");
        }
    }

    public async Task<Result<IEnumerable<PerformanceReviewListDto>>> GetByReviewerIdAsync(int reviewerId)
    {
        try
        {
            var reviews = await _unitOfWork.PerformanceReviews.GetByReviewerIdAsync(reviewerId);
            var dtos = _mapper.Map<IEnumerable<PerformanceReviewListDto>>(reviews);
            return Result<IEnumerable<PerformanceReviewListDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting performance reviews for reviewer: {ReviewerId}", reviewerId);
            return Result<IEnumerable<PerformanceReviewListDto>>.Fail("Değerlendiren kişinin değerlendirmeleri getirilirken hata oluştu");
        }
    }

    public async Task<Result<IEnumerable<PerformanceReviewListDto>>> GetByReviewPeriodIdAsync(int reviewPeriodId)
    {
        try
        {
            var reviews = await _unitOfWork.PerformanceReviews.GetByReviewPeriodIdAsync(reviewPeriodId);
            var dtos = _mapper.Map<IEnumerable<PerformanceReviewListDto>>(reviews);
            return Result<IEnumerable<PerformanceReviewListDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting performance reviews for period: {PeriodId}", reviewPeriodId);
            return Result<IEnumerable<PerformanceReviewListDto>>.Fail("Dönem değerlendirmeleri getirilirken hata oluştu");
        }
    }

    public async Task<Result<IEnumerable<PerformanceReviewListDto>>> GetByStatusAsync(ReviewStatus status)
    {
        try
        {
            var reviews = await _unitOfWork.PerformanceReviews.GetByStatusAsync(status);
            var dtos = _mapper.Map<IEnumerable<PerformanceReviewListDto>>(reviews);
            return Result<IEnumerable<PerformanceReviewListDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting performance reviews by status: {Status}", status);
            return Result<IEnumerable<PerformanceReviewListDto>>.Fail("Durum değerlendirmeleri getirilirken hata oluştu");
        }
    }

    public async Task<Result<IEnumerable<PerformanceReviewListDto>>> GetPendingReviewsAsync()
    {
        try
        {
            var reviews = await _unitOfWork.PerformanceReviews.GetPendingReviewsAsync();
            var dtos = _mapper.Map<IEnumerable<PerformanceReviewListDto>>(reviews);
            return Result<IEnumerable<PerformanceReviewListDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting pending performance reviews");
            return Result<IEnumerable<PerformanceReviewListDto>>.Fail("Bekleyen değerlendirmeler getirilirken hata oluştu");
        }
    }

    public async Task<Result<IEnumerable<PerformanceReviewListDto>>> GetCompletedReviewsAsync()
    {
        try
        {
            var reviews = await _unitOfWork.PerformanceReviews.GetCompletedReviewsAsync();
            var dtos = _mapper.Map<IEnumerable<PerformanceReviewListDto>>(reviews);
            return Result<IEnumerable<PerformanceReviewListDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting completed performance reviews");
            return Result<IEnumerable<PerformanceReviewListDto>>.Fail("Tamamlanan değerlendirmeler getirilirken hata oluştu");
        }
    }

    public async Task<Result<IEnumerable<PerformanceReviewListDto>>> GetByDepartmentIdAsync(int departmentId)
    {
        try
        {
            var reviews = await _unitOfWork.PerformanceReviews.GetByDepartmentIdAsync(departmentId);
            var dtos = _mapper.Map<IEnumerable<PerformanceReviewListDto>>(reviews);
            return Result<IEnumerable<PerformanceReviewListDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting performance reviews for department: {DepartmentId}", departmentId);
            return Result<IEnumerable<PerformanceReviewListDto>>.Fail("Departman değerlendirmeleri getirilirken hata oluştu");
        }
    }

    #endregion

    #region Review Status Management

    public async Task<Result> SubmitReviewAsync(int reviewId)
    {
        try
        {
            var review = await _unitOfWork.PerformanceReviews.GetByIdAsync(reviewId);
            if (review == null)
                return Result.Fail("Performans değerlendirmesi bulunamadı");

            if (review.Status != ReviewStatus.Draft && review.Status != ReviewStatus.InProgress)
                return Result.Fail("Sadece taslak veya devam eden değerlendirmeler gönderilebilir");

            review.Status = ReviewStatus.ManagerReview;
            review.SubmittedAt = DateTime.Now;
            review.UpdatedAt = DateTime.Now;

            _unitOfWork.PerformanceReviews.Update(review);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Performance review submitted: {ReviewId}", reviewId);
            return Result.Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error submitting performance review: {ReviewId}", reviewId);
            return Result.Fail("Performans değerlendirmesi gönderilirken hata oluştu");
        }
    }

    public async Task<Result> ApproveReviewAsync(int reviewId, int approverId)
    {
        try
        {
            var review = await _unitOfWork.PerformanceReviews.GetByIdAsync(reviewId);
            if (review == null)
                return Result.Fail("Performans değerlendirmesi bulunamadı");

            if (review.Status != ReviewStatus.Completed)
                return Result.Fail("Sadece tamamlanan değerlendirmeler onaylanabilir");

            var canApprove = await CanApproveReviewAsync(reviewId, approverId);
            if (!canApprove.IsSuccess || !canApprove.Data)
                return Result.Fail("Bu değerlendirmeyi onaylama yetkiniz yok");

            review.Status = ReviewStatus.Approved;
            review.ApprovedAt = DateTime.Now;
            review.ApprovedById = approverId;
            review.UpdatedAt = DateTime.Now;

            _unitOfWork.PerformanceReviews.Update(review);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Performance review approved: {ReviewId} by {ApproverId}", reviewId, approverId);
            return Result.Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error approving performance review: {ReviewId}", reviewId);
            return Result.Fail("Performans değerlendirmesi onaylanırken hata oluştu");
        }
    }

    public async Task<Result> RejectReviewAsync(int reviewId, string reason)
    {
        try
        {
            var review = await _unitOfWork.PerformanceReviews.GetByIdAsync(reviewId);
            if (review == null)
                return Result.Fail("Performans değerlendirmesi bulunamadı");

            review.Status = ReviewStatus.InProgress;
            review.ReviewerComments = $"Red nedeni: {reason}";
            review.UpdatedAt = DateTime.Now;

            _unitOfWork.PerformanceReviews.Update(review);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Performance review rejected: {ReviewId}, Reason: {Reason}", reviewId, reason);
            return Result.Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error rejecting performance review: {ReviewId}", reviewId);
            return Result.Fail("Performans değerlendirmesi reddedilirken hata oluştu");
        }
    }

    public async Task<Result> StartEmployeeReviewAsync(int reviewId)
    {
        try
        {
            var review = await _unitOfWork.PerformanceReviews.GetByIdAsync(reviewId);
            if (review == null)
                return Result.Fail("Performans değerlendirmesi bulunamadı");

            if (review.Status != ReviewStatus.Draft)
                return Result.Fail("Sadece taslak durumundaki değerlendirmeler başlatılabilir");

            review.Status = ReviewStatus.EmployeeReview;
            review.UpdatedAt = DateTime.Now;

            _unitOfWork.PerformanceReviews.Update(review);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Employee review started: {ReviewId}", reviewId);
            return Result.Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error starting employee review: {ReviewId}", reviewId);
            return Result.Fail("Çalışan değerlendirmesi başlatılırken hata oluştu");
        }
    }

    public async Task<Result> CompleteEmployeeReviewAsync(int reviewId)
    {
        try
        {
            var review = await _unitOfWork.PerformanceReviews.GetByIdAsync(reviewId);
            if (review == null)
                return Result.Fail("Performans değerlendirmesi bulunamadı");

            if (review.Status != ReviewStatus.ManagerReview)
                return Result.Fail("Sadece yönetici değerlendirmesi tamamlanan değerlendirmeler bitirilebilir");

            review.Status = ReviewStatus.Completed;
            review.UpdatedAt = DateTime.Now;

            _unitOfWork.PerformanceReviews.Update(review);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Performance review completed: {ReviewId}", reviewId);
            return Result.Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error completing performance review: {ReviewId}", reviewId);
            return Result.Fail("Performans değerlendirmesi tamamlanırken hata oluştu");
        }
    }

    #endregion

    #region Self Assessment

    public async Task<Result> CompleteSelfAssessmentAsync(SelfAssessmentDto dto)
    {
        try
        {
            var review = await _unitOfWork.PerformanceReviews.GetByIdAsync(dto.PerformanceReviewId);
            if (review == null)
                return Result.Fail("Performans değerlendirmesi bulunamadı");

            review.IsSelfAssessmentCompleted = true;
            review.SelfAssessmentCompletedAt = DateTime.Now;
            review.SelfOverallScore = dto.SelfOverallScore;
            review.SelfAssessmentComments = dto.SelfAssessmentComments;
            review.EmployeeComments = dto.EmployeeComments;
            review.UpdatedAt = DateTime.Now;

            if (review.Status == ReviewStatus.EmployeeReview)
            {
                review.Status = ReviewStatus.ManagerReview;
            }

            _unitOfWork.PerformanceReviews.Update(review);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Self assessment completed for review: {ReviewId}", dto.PerformanceReviewId);
            return Result.Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error completing self assessment for review: {ReviewId}", dto.PerformanceReviewId);
            return Result.Fail("Öz değerlendirme tamamlanırken hata oluştu");
        }
    }

    public async Task<Result<PerformanceReviewDetailDto?>> GetSelfAssessmentAsync(int personId, int reviewPeriodId)
    {
        try
        {
            var review = await _unitOfWork.PerformanceReviews.GetByPersonAndPeriodAsync(personId, reviewPeriodId);
            if (review == null)
                return Result<PerformanceReviewDetailDto?>.Fail("Bu dönem için değerlendirme bulunamadı");

            var dto = _mapper.Map<PerformanceReviewDetailDto>(review);
            return Result<PerformanceReviewDetailDto?>.Ok(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting self assessment for person: {PersonId}, period: {PeriodId}", personId, reviewPeriodId);
            return Result<PerformanceReviewDetailDto?>.Fail("Öz değerlendirme getirilirken hata oluştu");
        }
    }

    #endregion

    #region Analytics and Reporting

    public async Task<Result<PerformanceAnalyticsDto>> GetPerformanceAnalyticsAsync(int? departmentId = null, int? year = null)
    {
        try
        {
            var analytics = new PerformanceAnalyticsDto();
            
            // Basic implementation - can be expanded with more complex analytics
            var allReviews = await _unitOfWork.PerformanceReviews.GetAllAsync();
            
            if (departmentId.HasValue)
            {
                allReviews = allReviews.Where(r => r.Person.DepartmentId == departmentId.Value);
            }
            
            if (year.HasValue)
            {
                allReviews = allReviews.Where(r => r.ReviewPeriod.StartDate.Year == year.Value);
            }

            var reviewsList = allReviews.ToList();
            
            analytics.TotalReviews = reviewsList.Count;
            analytics.CompletedReviews = reviewsList.Count(r => r.Status == ReviewStatus.Completed || r.Status == ReviewStatus.Approved);
            analytics.PendingReviews = analytics.TotalReviews - analytics.CompletedReviews;
            analytics.CompletionRate = analytics.TotalReviews > 0 ? (decimal)analytics.CompletedReviews / analytics.TotalReviews * 100 : 0;

            var completedReviews = reviewsList.Where(r => r.Status == ReviewStatus.Completed || r.Status == ReviewStatus.Approved).ToList();
            
            if (completedReviews.Any())
            {
                analytics.AverageOverallScore = (decimal)completedReviews.Average(r => r.OverallScore);
                analytics.AverageJobQualityScore = (decimal)completedReviews.Average(r => r.JobQualityScore);
                analytics.AverageProductivityScore = (decimal)completedReviews.Average(r => r.ProductivityScore);
                analytics.AverageTeamworkScore = (decimal)completedReviews.Average(r => r.TeamworkScore);
                analytics.AverageCommunicationScore = (decimal)completedReviews.Average(r => r.CommunicationScore);
                analytics.AverageLeadershipScore = (decimal)completedReviews.Average(r => r.LeadershipScore);
            }

            return Result<PerformanceAnalyticsDto>.Ok(analytics);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting performance analytics");
            return Result<PerformanceAnalyticsDto>.Fail("Performans analitiği getirilirken hata oluştu");
        }
    }

    public async Task<Result<decimal>> GetAverageScoreByPersonAsync(int personId)
    {
        try
        {
            var averageScore = await _unitOfWork.PerformanceReviews.GetAverageScoreByPersonAsync(personId);
            return Result<decimal>.Ok(averageScore);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting average score for person: {PersonId}", personId);
            return Result<decimal>.Fail("Personel ortalama puanı hesaplanırken hata oluştu");
        }
    }

    public async Task<Result<decimal>> GetAverageScoreByDepartmentAsync(int departmentId)
    {
        try
        {
            var averageScore = await _unitOfWork.PerformanceReviews.GetAverageScoreByDepartmentAsync(departmentId);
            return Result<decimal>.Ok(averageScore);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting average score for department: {DepartmentId}", departmentId);
            return Result<decimal>.Fail("Departman ortalama puanı hesaplanırken hata oluştu");
        }
    }

    public async Task<Result<IEnumerable<PerformanceReviewListDto>>> GetReviewsForApprovalAsync(int approverId)
    {
        try
        {
            var reviews = await _unitOfWork.PerformanceReviews.GetReviewsForApprovalAsync(approverId);
            var dtos = _mapper.Map<IEnumerable<PerformanceReviewListDto>>(reviews);
            return Result<IEnumerable<PerformanceReviewListDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting reviews for approval by: {ApproverId}", approverId);
            return Result<IEnumerable<PerformanceReviewListDto>>.Fail("Onay bekleyen değerlendirmeler getirilirken hata oluştu");
        }
    }

    #endregion

    #region Review Period Operations

    public async Task<Result<IEnumerable<ReviewPeriodListDto>>> GetAllPeriodsAsync()
    {
        try
        {
            var periods = await _unitOfWork.ReviewPeriods.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<ReviewPeriodListDto>>(periods);
            return Result<IEnumerable<ReviewPeriodListDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all review periods");
            return Result<IEnumerable<ReviewPeriodListDto>>.Fail("Değerlendirme dönemleri getirilirken hata oluştu");
        }
    }

    public async Task<Result<ReviewPeriodDetailDto?>> GetPeriodByIdAsync(int id)
    {
        try
        {
            var period = await _unitOfWork.ReviewPeriods.GetByIdAsync(id);
            if (period == null)
                return Result<ReviewPeriodDetailDto?>.Fail("Değerlendirme dönemi bulunamadı");

            var dto = _mapper.Map<ReviewPeriodDetailDto>(period);
            return Result<ReviewPeriodDetailDto?>.Ok(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting review period by id: {Id}", id);
            return Result<ReviewPeriodDetailDto?>.Fail("Değerlendirme dönemi getirilirken hata oluştu");
        }
    }

    public async Task<Result<ReviewPeriodDetailDto>> CreatePeriodAsync(ReviewPeriodCreateDto dto)
    {
        try
        {
            // Validation
            var isNameUnique = await _unitOfWork.ReviewPeriods.IsNameUniqueAsync(dto.Name);
            if (!isNameUnique)
                return Result<ReviewPeriodDetailDto>.Fail("Bu isimde bir değerlendirme dönemi zaten mevcut");

            var hasOverlapping = await _unitOfWork.ReviewPeriods.HasOverlappingPeriodAsync(dto.StartDate, dto.EndDate);
            if (hasOverlapping)
                return Result<ReviewPeriodDetailDto>.Fail("Bu tarih aralığında çakışan bir dönem mevcut");

            var period = _mapper.Map<ReviewPeriod>(dto);
            period.CreatedAt = DateTime.Now;

            await _unitOfWork.ReviewPeriods.AddAsync(period);
            await _unitOfWork.SaveChangesAsync();

            var result = await GetPeriodByIdAsync(period.Id);
            _logger.LogInformation("Review period created: {PeriodName}", dto.Name);
            return Result<ReviewPeriodDetailDto>.Ok(result.Data!);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating review period");
            return Result<ReviewPeriodDetailDto>.Fail("Değerlendirme dönemi oluşturulurken hata oluştu");
        }
    }

    public async Task<Result<ReviewPeriodDetailDto>> UpdatePeriodAsync(ReviewPeriodUpdateDto dto)
    {
        try
        {
            var period = await _unitOfWork.ReviewPeriods.GetByIdAsync(dto.Id);
            if (period == null)
                return Result<ReviewPeriodDetailDto>.Fail("Değerlendirme dönemi bulunamadı");

            // Validation
            var isNameUnique = await _unitOfWork.ReviewPeriods.IsNameUniqueAsync(dto.Name, dto.Id);
            if (!isNameUnique)
                return Result<ReviewPeriodDetailDto>.Fail("Bu isimde bir değerlendirme dönemi zaten mevcut");

            var hasOverlapping = await _unitOfWork.ReviewPeriods.HasOverlappingPeriodAsync(dto.StartDate, dto.EndDate, dto.Id);
            if (hasOverlapping)
                return Result<ReviewPeriodDetailDto>.Fail("Bu tarih aralığında çakışan bir dönem mevcut");

            _mapper.Map(dto, period);
            period.UpdatedAt = DateTime.Now;

            _unitOfWork.ReviewPeriods.Update(period);
            await _unitOfWork.SaveChangesAsync();

            var result = await GetPeriodByIdAsync(dto.Id);
            _logger.LogInformation("Review period updated: {PeriodId}", dto.Id);
            return Result<ReviewPeriodDetailDto>.Ok(result.Data!);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating review period: {PeriodId}", dto.Id);
            return Result<ReviewPeriodDetailDto>.Fail("Değerlendirme dönemi güncellenirken hata oluştu");
        }
    }

    public async Task<Result> DeletePeriodAsync(int id)
    {
        try
        {
            var period = await _unitOfWork.ReviewPeriods.GetByIdAsync(id);
            if (period == null)
                return Result.Fail("Değerlendirme dönemi bulunamadı");

            if (period.PerformanceReviews.Any())
                return Result.Fail("Bu dönemde değerlendirmeler mevcut, dönem silinemez");

            _unitOfWork.ReviewPeriods.Remove(period);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Review period deleted: {PeriodId}", id);
            return Result.Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting review period: {PeriodId}", id);
            return Result.Fail("Değerlendirme dönemi silinirken hata oluştu");
        }
    }

    public async Task<Result<IEnumerable<ReviewPeriodListDto>>> GetActivePeriodsAsync()
    {
        try
        {
            var periods = await _unitOfWork.ReviewPeriods.GetActivePeriodsAsync();
            var dtos = _mapper.Map<IEnumerable<ReviewPeriodListDto>>(periods);
            return Result<IEnumerable<ReviewPeriodListDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting active review periods");
            return Result<IEnumerable<ReviewPeriodListDto>>.Fail("Aktif değerlendirme dönemleri getirilirken hata oluştu");
        }
    }

    public async Task<Result<ReviewPeriodDetailDto?>> GetCurrentPeriodAsync()
    {
        try
        {
            var period = await _unitOfWork.ReviewPeriods.GetCurrentPeriodAsync();
            if (period == null)
                return Result<ReviewPeriodDetailDto?>.Ok(null);

            var dto = _mapper.Map<ReviewPeriodDetailDto>(period);
            return Result<ReviewPeriodDetailDto?>.Ok(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting current review period");
            return Result<ReviewPeriodDetailDto?>.Fail("Mevcut değerlendirme dönemi getirilirken hata oluştu");
        }
    }

    #endregion

    #region Goal Management

    public async Task<Result<IEnumerable<PerformanceGoalDto>>> GetGoalsByReviewIdAsync(int performanceReviewId)
    {
        try
        {
            var goals = await _unitOfWork.PerformanceGoals.GetByPerformanceReviewIdAsync(performanceReviewId);
            var dtos = _mapper.Map<IEnumerable<PerformanceGoalDto>>(goals);
            return Result<IEnumerable<PerformanceGoalDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting goals for review: {ReviewId}", performanceReviewId);
            return Result<IEnumerable<PerformanceGoalDto>>.Fail("Değerlendirme hedefleri getirilirken hata oluştu");
        }
    }

    public async Task<Result<IEnumerable<PerformanceGoalDto>>> GetGoalsByPersonIdAsync(int personId)
    {
        try
        {
            var goals = await _unitOfWork.PerformanceGoals.GetByPersonIdAsync(personId);
            var dtos = _mapper.Map<IEnumerable<PerformanceGoalDto>>(goals);
            return Result<IEnumerable<PerformanceGoalDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting goals for person: {PersonId}", personId);
            return Result<IEnumerable<PerformanceGoalDto>>.Fail("Personel hedefleri getirilirken hata oluştu");
        }
    }

    public async Task<Result<PerformanceGoalDto>> CreateGoalAsync(int performanceReviewId, PerformanceGoalCreateDto dto)
    {
        try
        {
            var review = await _unitOfWork.PerformanceReviews.GetByIdAsync(performanceReviewId);
            if (review == null)
                return Result<PerformanceGoalDto>.Fail("Performans değerlendirmesi bulunamadı");

            var goal = _mapper.Map<PerformanceGoal>(dto);
            goal.PerformanceReviewId = performanceReviewId;
            goal.Status = GoalStatus.NotStarted;
            goal.CreatedAt = DateTime.Now;

            await _unitOfWork.PerformanceGoals.AddAsync(goal);
            await _unitOfWork.SaveChangesAsync();

            var resultDto = _mapper.Map<PerformanceGoalDto>(goal);
            _logger.LogInformation("Performance goal created for review: {ReviewId}", performanceReviewId);
            return Result<PerformanceGoalDto>.Ok(resultDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating performance goal");
            return Result<PerformanceGoalDto>.Fail("Performans hedefi oluşturulurken hata oluştu");
        }
    }

    public async Task<Result<PerformanceGoalDto>> UpdateGoalAsync(PerformanceGoalUpdateDto dto)
    {
        try
        {
            var goal = await _unitOfWork.PerformanceGoals.GetByIdAsync(dto.Id);
            if (goal == null)
                return Result<PerformanceGoalDto>.Fail("Performans hedefi bulunamadı");

            _mapper.Map(dto, goal);
            goal.UpdatedAt = DateTime.Now;

            _unitOfWork.PerformanceGoals.Update(goal);
            await _unitOfWork.SaveChangesAsync();

            var resultDto = _mapper.Map<PerformanceGoalDto>(goal);
            _logger.LogInformation("Performance goal updated: {GoalId}", dto.Id);
            return Result<PerformanceGoalDto>.Ok(resultDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating performance goal: {GoalId}", dto.Id);
            return Result<PerformanceGoalDto>.Fail("Performans hedefi güncellenirken hata oluştu");
        }
    }

    public async Task<Result> DeleteGoalAsync(int goalId)
    {
        try
        {
            var goal = await _unitOfWork.PerformanceGoals.GetByIdAsync(goalId);
            if (goal == null)
                return Result.Fail("Performans hedefi bulunamadı");

            _unitOfWork.PerformanceGoals.Remove(goal);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Performance goal deleted: {GoalId}", goalId);
            return Result.Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting performance goal: {GoalId}", goalId);
            return Result.Fail("Performans hedefi silinirken hata oluştu");
        }
    }

    public async Task<Result<IEnumerable<PerformanceGoalDto>>> GetOverdueGoalsAsync()
    {
        try
        {
            var goals = await _unitOfWork.PerformanceGoals.GetOverdueGoalsAsync();
            var dtos = _mapper.Map<IEnumerable<PerformanceGoalDto>>(goals);
            return Result<IEnumerable<PerformanceGoalDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting overdue goals");
            return Result<IEnumerable<PerformanceGoalDto>>.Fail("Gecikmiş hedefler getirilirken hata oluştu");
        }
    }

    public async Task<Result<IEnumerable<PerformanceGoalDto>>> GetUpcomingGoalsAsync(int days = 30)
    {
        try
        {
            var goals = await _unitOfWork.PerformanceGoals.GetUpcomingGoalsAsync(days);
            var dtos = _mapper.Map<IEnumerable<PerformanceGoalDto>>(goals);
            return Result<IEnumerable<PerformanceGoalDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting upcoming goals");
            return Result<IEnumerable<PerformanceGoalDto>>.Fail("Yaklaşan hedefler getirilirken hata oluştu");
        }
    }

    #endregion

    #region Validation

    public async Task<Result<bool>> CanCreateReviewAsync(int personId, int reviewPeriodId)
    {
        try
        {
            var existingReview = await _unitOfWork.PerformanceReviews.GetByPersonAndPeriodAsync(personId, reviewPeriodId);
            return Result<bool>.Ok(existingReview == null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if review can be created");
            return Result<bool>.Fail("Değerlendirme oluşturma kontrolü yapılırken hata oluştu");
        }
    }

    public async Task<Result<bool>> CanEditReviewAsync(int reviewId, int userId)
    {
        try
        {
            var review = await _unitOfWork.PerformanceReviews.GetByIdAsync(reviewId);
            if (review == null)
                return Result<bool>.Ok(false);

            // Can edit if: reviewer, person being reviewed, or admin
            var canEdit = review.ReviewerId == userId || 
                         review.PersonId == userId || 
                         review.Status == ReviewStatus.Draft;

            return Result<bool>.Ok(canEdit);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if review can be edited");
            return Result<bool>.Fail("Değerlendirme düzenleme kontrolü yapılırken hata oluştu");
        }
    }

    public async Task<Result<bool>> CanApproveReviewAsync(int reviewId, int approverId)
    {
        try
        {
            var review = await _unitOfWork.PerformanceReviews.GetByIdAsync(reviewId);
            if (review == null)
                return Result<bool>.Ok(false);

            // Simple approval logic - can be expanded based on business rules
            var canApprove = review.Status == ReviewStatus.Completed;
            return Result<bool>.Ok(canApprove);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if review can be approved");
            return Result<bool>.Fail("Değerlendirme onaylama kontrolü yapılırken hata oluştu");
        }
    }

    #endregion
}
