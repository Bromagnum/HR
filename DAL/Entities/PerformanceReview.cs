using System.ComponentModel.DataAnnotations;

namespace DAL.Entities;

public class PerformanceReview : BaseEntity
{
    [Required]
    public int PersonId { get; set; }
    
    [Required]
    public int ReviewPeriodId { get; set; }
    
    [Required]
    public int ReviewerId { get; set; }
    
    // Genel Değerlendirme Skoru (1-5)
    [Range(1, 5)]
    public int OverallScore { get; set; }
    
    // Kategori Bazlı Skorlar (1-5)
    [Range(1, 5)]
    public int JobQualityScore { get; set; }
    
    [Range(1, 5)]
    public int ProductivityScore { get; set; }
    
    [Range(1, 5)]
    public int TeamworkScore { get; set; }
    
    [Range(1, 5)]
    public int CommunicationScore { get; set; }
    
    [Range(1, 5)]
    public int LeadershipScore { get; set; }
    
    [Range(1, 5)]
    public int InitiativeScore { get; set; }
    
    [Range(1, 5)]
    public int ProblemSolvingScore { get; set; }
    
    [Range(1, 5)]
    public int AdaptabilityScore { get; set; }
    
    // Yorumlar ve Geri Bildirimler
    [StringLength(2000)]
    public string? Strengths { get; set; }
    
    [StringLength(2000)]
    public string? AreasForImprovement { get; set; }
    
    [StringLength(2000)]
    public string? Achievements { get; set; }
    
    [StringLength(2000)]
    public string? Goals { get; set; }
    
    [StringLength(2000)]
    public string? ReviewerComments { get; set; }
    
    [StringLength(2000)]
    public string? EmployeeComments { get; set; }
    
    // Durumlar
    public ReviewStatus Status { get; set; } = ReviewStatus.Draft;
    
    public DateTime? SubmittedAt { get; set; }
    
    public DateTime? ApprovedAt { get; set; }
    
    public int? ApprovedById { get; set; }
    
    // Self Assessment (Öz Değerlendirme)
    public bool IsSelfAssessmentCompleted { get; set; }
    
    public DateTime? SelfAssessmentCompletedAt { get; set; }
    
    [Range(1, 5)]
    public int? SelfOverallScore { get; set; }
    
    [StringLength(2000)]
    public string? SelfAssessmentComments { get; set; }
    
    // Navigation Properties
    public virtual Person Person { get; set; } = null!;
    public virtual ReviewPeriod ReviewPeriod { get; set; } = null!;
    public virtual Person Reviewer { get; set; } = null!;
    public virtual Person? ApprovedBy { get; set; }
    public virtual ICollection<PerformanceGoal> Goals_Navigation { get; set; } = new List<PerformanceGoal>();
}

public enum ReviewStatus
{
    Draft = 0,
    InProgress = 1,
    EmployeeReview = 2,
    ManagerReview = 3,
    Completed = 4,
    Approved = 5
}

public class ReviewPeriod : BaseEntity
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [StringLength(500)]
    public string? Description { get; set; }
    
    [Required]
    public DateTime StartDate { get; set; }
    
    [Required]
    public DateTime EndDate { get; set; }
    
    [Required]
    public DateTime ReviewStartDate { get; set; }
    
    [Required]
    public DateTime ReviewEndDate { get; set; }
    
    public new bool IsActive { get; set; } = true;
    
    public ReviewPeriodType Type { get; set; } = ReviewPeriodType.Annual;
    
    // Navigation Properties
    public virtual ICollection<PerformanceReview> PerformanceReviews { get; set; } = new List<PerformanceReview>();
}

public enum ReviewPeriodType
{
    Monthly = 1,
    Quarterly = 2,
    SemiAnnual = 3,
    Annual = 4,
    Custom = 5
}

public class PerformanceGoal : BaseEntity
{
    [Required]
    public int PerformanceReviewId { get; set; }
    
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;
    
    [StringLength(1000)]
    public string? Description { get; set; }
    
    [Required]
    public DateTime TargetDate { get; set; }
    
    public GoalStatus Status { get; set; } = GoalStatus.NotStarted;
    
    [Range(0, 100)]
    public int ProgressPercentage { get; set; } = 0;
    
    [StringLength(1000)]
    public string? Notes { get; set; }
    
    public GoalPriority Priority { get; set; } = GoalPriority.Medium;
    
    // Navigation Properties
    public virtual PerformanceReview PerformanceReview { get; set; } = null!;
}

public enum GoalStatus
{
    NotStarted = 0,
    InProgress = 1,
    Completed = 2,
    OnHold = 3,
    Cancelled = 4
}

public enum GoalPriority
{
    Low = 1,
    Medium = 2,
    High = 3,
    Critical = 4
}
