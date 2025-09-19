using System.ComponentModel.DataAnnotations;
using DAL.Entities;

namespace BLL.DTOs;

public class PerformanceReviewListDto
{
    public int Id { get; set; }
    public int PersonId { get; set; }
    public string PersonName { get; set; } = string.Empty;
    public string DepartmentName { get; set; } = string.Empty;
    public int ReviewPeriodId { get; set; }
    public string ReviewPeriodName { get; set; } = string.Empty;
    public int ReviewerId { get; set; }
    public string ReviewerName { get; set; } = string.Empty;
    public int OverallScore { get; set; }
    public ReviewStatus Status { get; set; }
    public string StatusText { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? SubmittedAt { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public bool IsSelfAssessmentCompleted { get; set; }
}

public class PerformanceReviewDetailDto
{
    public int Id { get; set; }
    public int PersonId { get; set; }
    public string PersonName { get; set; } = string.Empty;
    public string PersonEmail { get; set; } = string.Empty;
    public string DepartmentName { get; set; } = string.Empty;
    public string PositionName { get; set; } = string.Empty;
    public int ReviewPeriodId { get; set; }
    public string ReviewPeriodName { get; set; } = string.Empty;
    public DateTime ReviewPeriodStart { get; set; }
    public DateTime ReviewPeriodEnd { get; set; }
    public int ReviewerId { get; set; }
    public string ReviewerName { get; set; } = string.Empty;

    // Skorlar
    public int OverallScore { get; set; }
    public int JobQualityScore { get; set; }
    public int ProductivityScore { get; set; }
    public int TeamworkScore { get; set; }
    public int CommunicationScore { get; set; }
    public int LeadershipScore { get; set; }
    public int InitiativeScore { get; set; }
    public int ProblemSolvingScore { get; set; }
    public int AdaptabilityScore { get; set; }

    // Yorumlar
    public string? Strengths { get; set; }
    public string? AreasForImprovement { get; set; }
    public string? Achievements { get; set; }
    public string? Goals { get; set; }
    public string? ReviewerComments { get; set; }
    public string? EmployeeComments { get; set; }

    // Durum
    public ReviewStatus Status { get; set; }
    public string StatusText { get; set; } = string.Empty;
    public DateTime? SubmittedAt { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public string? ApprovedByName { get; set; }

    // Öz Değerlendirme
    public bool IsSelfAssessmentCompleted { get; set; }
    public DateTime? SelfAssessmentCompletedAt { get; set; }
    public int? SelfOverallScore { get; set; }
    public string? SelfAssessmentComments { get; set; }

    // Hedefler
    public List<PerformanceGoalDto> Goals_List { get; set; } = new();

    // Metadata
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class PerformanceReviewCreateDto
{
    [Required]
    public int PersonId { get; set; }

    [Required]
    public int ReviewPeriodId { get; set; }

    [Required]
    public int ReviewerId { get; set; }

    [Range(1, 5, ErrorMessage = "Skor 1-5 arasında olmalıdır")]
    public int OverallScore { get; set; } = 3;

    [Range(1, 5, ErrorMessage = "Skor 1-5 arasında olmalıdır")]
    public int JobQualityScore { get; set; } = 3;

    [Range(1, 5, ErrorMessage = "Skor 1-5 arasında olmalıdır")]
    public int ProductivityScore { get; set; } = 3;

    [Range(1, 5, ErrorMessage = "Skor 1-5 arasında olmalıdır")]
    public int TeamworkScore { get; set; } = 3;

    [Range(1, 5, ErrorMessage = "Skor 1-5 arasında olmalıdır")]
    public int CommunicationScore { get; set; } = 3;

    [Range(1, 5, ErrorMessage = "Skor 1-5 arasında olmalıdır")]
    public int LeadershipScore { get; set; } = 3;

    [Range(1, 5, ErrorMessage = "Skor 1-5 arasında olmalıdır")]
    public int InitiativeScore { get; set; } = 3;

    [Range(1, 5, ErrorMessage = "Skor 1-5 arasında olmalıdır")]
    public int ProblemSolvingScore { get; set; } = 3;

    [Range(1, 5, ErrorMessage = "Skor 1-5 arasında olmalıdır")]
    public int AdaptabilityScore { get; set; } = 3;

    [StringLength(2000, ErrorMessage = "Güçlü yönler en fazla 2000 karakter olabilir")]
    public string? Strengths { get; set; }

    [StringLength(2000, ErrorMessage = "Gelişim alanları en fazla 2000 karakter olabilir")]
    public string? AreasForImprovement { get; set; }

    [StringLength(2000, ErrorMessage = "Başarılar en fazla 2000 karakter olabilir")]
    public string? Achievements { get; set; }

    [StringLength(2000, ErrorMessage = "Hedefler en fazla 2000 karakter olabilir")]
    public string? Goals { get; set; }

    [StringLength(2000, ErrorMessage = "Değerlendiren yorumları en fazla 2000 karakter olabilir")]
    public string? ReviewerComments { get; set; }

    public List<PerformanceGoalCreateDto> Goals_List { get; set; } = new();
}

public class PerformanceReviewUpdateDto
{
    [Required]
    public int Id { get; set; }

    [Range(1, 5, ErrorMessage = "Skor 1-5 arasında olmalıdır")]
    public int OverallScore { get; set; }

    [Range(1, 5, ErrorMessage = "Skor 1-5 arasında olmalıdır")]
    public int JobQualityScore { get; set; }

    [Range(1, 5, ErrorMessage = "Skor 1-5 arasında olmalıdır")]
    public int ProductivityScore { get; set; }

    [Range(1, 5, ErrorMessage = "Skor 1-5 arasında olmalıdır")]
    public int TeamworkScore { get; set; }

    [Range(1, 5, ErrorMessage = "Skor 1-5 arasında olmalıdır")]
    public int CommunicationScore { get; set; }

    [Range(1, 5, ErrorMessage = "Skor 1-5 arasında olmalıdır")]
    public int LeadershipScore { get; set; }

    [Range(1, 5, ErrorMessage = "Skor 1-5 arasında olmalıdır")]
    public int InitiativeScore { get; set; }

    [Range(1, 5, ErrorMessage = "Skor 1-5 arasında olmalıdır")]
    public int ProblemSolvingScore { get; set; }

    [Range(1, 5, ErrorMessage = "Skor 1-5 arasında olmalıdır")]
    public int AdaptabilityScore { get; set; }

    [StringLength(2000, ErrorMessage = "Güçlü yönler en fazla 2000 karakter olabilir")]
    public string? Strengths { get; set; }

    [StringLength(2000, ErrorMessage = "Gelişim alanları en fazla 2000 karakter olabilir")]
    public string? AreasForImprovement { get; set; }

    [StringLength(2000, ErrorMessage = "Başarılar en fazla 2000 karakter olabilir")]
    public string? Achievements { get; set; }

    [StringLength(2000, ErrorMessage = "Hedefler en fazla 2000 karakter olabilir")]
    public string? Goals { get; set; }

    [StringLength(2000, ErrorMessage = "Değerlendiren yorumları en fazla 2000 karakter olabilir")]
    public string? ReviewerComments { get; set; }
}

public class SelfAssessmentDto
{
    [Required]
    public int PerformanceReviewId { get; set; }

    [Range(1, 5, ErrorMessage = "Skor 1-5 arasında olmalıdır")]
    public int SelfOverallScore { get; set; }

    [StringLength(2000, ErrorMessage = "Öz değerlendirme yorumları en fazla 2000 karakter olabilir")]
    public string? SelfAssessmentComments { get; set; }

    [StringLength(2000, ErrorMessage = "Çalışan yorumları en fazla 2000 karakter olabilir")]
    public string? EmployeeComments { get; set; }
}

public class ReviewPeriodListDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime ReviewStartDate { get; set; }
    public DateTime ReviewEndDate { get; set; }
    public bool IsActive { get; set; }
    public ReviewPeriodType Type { get; set; }
    public string TypeText { get; set; } = string.Empty;
    public int ReviewCount { get; set; }
    public int CompletedReviewCount { get; set; }
    public bool IsCurrentPeriod { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class ReviewPeriodDetailDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime ReviewStartDate { get; set; }
    public DateTime ReviewEndDate { get; set; }
    public bool IsActive { get; set; }
    public ReviewPeriodType Type { get; set; }
    public string TypeText { get; set; } = string.Empty;
    public List<PerformanceReviewListDto> PerformanceReviews { get; set; } = new();
    public int ReviewCount { get; set; }
    public int CompletedReviewCount { get; set; }
    public decimal CompletionPercentage { get; set; }
    public bool IsCurrentPeriod { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class ReviewPeriodCreateDto
{
    [Required]
    [StringLength(100, ErrorMessage = "Ad en fazla 100 karakter olabilir")]
    public string Name { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir")]
    public string? Description { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    [Required]
    public DateTime ReviewStartDate { get; set; }

    [Required]
    public DateTime ReviewEndDate { get; set; }

    [Required]
    public ReviewPeriodType Type { get; set; }

    public bool IsActive { get; set; } = true;
}

public class ReviewPeriodUpdateDto
{
    [Required]
    public int Id { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "Ad en fazla 100 karakter olabilir")]
    public string Name { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir")]
    public string? Description { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    [Required]
    public DateTime ReviewStartDate { get; set; }

    [Required]
    public DateTime ReviewEndDate { get; set; }

    [Required]
    public ReviewPeriodType Type { get; set; }

    public bool IsActive { get; set; }
}

public class PerformanceGoalDto
{
    public int Id { get; set; }
    public int PerformanceReviewId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime TargetDate { get; set; }
    public GoalStatus Status { get; set; }
    public string StatusText { get; set; } = string.Empty;
    public int ProgressPercentage { get; set; }
    public string? Notes { get; set; }
    public GoalPriority Priority { get; set; }
    public string PriorityText { get; set; } = string.Empty;
    public bool IsOverdue { get; set; }
    public bool IsUpcoming { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class PerformanceGoalCreateDto
{
    [Required]
    [StringLength(200, ErrorMessage = "Başlık en fazla 200 karakter olabilir")]
    public string Title { get; set; } = string.Empty;

    [StringLength(1000, ErrorMessage = "Açıklama en fazla 1000 karakter olabilir")]
    public string? Description { get; set; }

    [Required]
    public DateTime TargetDate { get; set; }

    public GoalPriority Priority { get; set; } = GoalPriority.Medium;

    [StringLength(1000, ErrorMessage = "Notlar en fazla 1000 karakter olabilir")]
    public string? Notes { get; set; }
}

public class PerformanceGoalUpdateDto
{
    [Required]
    public int Id { get; set; }

    [Required]
    [StringLength(200, ErrorMessage = "Başlık en fazla 200 karakter olabilir")]
    public string Title { get; set; } = string.Empty;

    [StringLength(1000, ErrorMessage = "Açıklama en fazla 1000 karakter olabilir")]
    public string? Description { get; set; }

    [Required]
    public DateTime TargetDate { get; set; }

    public GoalStatus Status { get; set; }

    [Range(0, 100, ErrorMessage = "İlerleme yüzdesi 0-100 arasında olmalıdır")]
    public int ProgressPercentage { get; set; }

    public GoalPriority Priority { get; set; }

    [StringLength(1000, ErrorMessage = "Notlar en fazla 1000 karakter olabilir")]
    public string? Notes { get; set; }
}

public class PerformanceAnalyticsDto
{
    public int TotalReviews { get; set; }
    public int CompletedReviews { get; set; }
    public int PendingReviews { get; set; }
    public decimal CompletionRate { get; set; }
    public decimal AverageOverallScore { get; set; }
    public decimal AverageJobQualityScore { get; set; }
    public decimal AverageProductivityScore { get; set; }
    public decimal AverageTeamworkScore { get; set; }
    public decimal AverageCommunicationScore { get; set; }
    public decimal AverageLeadershipScore { get; set; }
    public List<DepartmentPerformanceDto> DepartmentPerformances { get; set; } = new();
    public List<MonthlyPerformanceDto> MonthlyTrends { get; set; } = new();
}

public class DepartmentPerformanceDto
{
    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
    public int ReviewCount { get; set; }
    public decimal AverageScore { get; set; }
    public decimal CompletionRate { get; set; }
}

public class MonthlyPerformanceDto
{
    public int Year { get; set; }
    public int Month { get; set; }
    public string MonthName { get; set; } = string.Empty;
    public int ReviewCount { get; set; }
    public decimal AverageScore { get; set; }
}
