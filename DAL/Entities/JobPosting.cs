using System.ComponentModel.DataAnnotations;

namespace DAL.Entities;

public enum EmploymentType
{
    FullTime = 1,       // Tam Zamanlı
    PartTime = 2,       // Yarı Zamanlı
    Contract = 3,       // Sözleşmeli
    Internship = 4,     // Staj
    Freelance = 5       // Serbest Çalışan
}

public class JobPosting : BaseEntity
{
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(5000)]
    public string Description { get; set; } = string.Empty;

    [Required]
    public int PositionId { get; set; }

    [Required]
    public int DepartmentId { get; set; }

    [Required]
    public JobPostingStatus Status { get; set; } = JobPostingStatus.Draft;

    [Required]
    public EmploymentType EmploymentType { get; set; } = EmploymentType.FullTime;

    [StringLength(1000)]
    public string? Requirements { get; set; }

    [StringLength(1000)]
    public string? Responsibilities { get; set; }

    [StringLength(1000)]
    public string? Benefits { get; set; }

    [Range(0, 50)]
    public int? MinExperience { get; set; }

    [Range(0, 50)]
    public int? MaxExperience { get; set; }

    [StringLength(100)]
    public string? MinEducation { get; set; }

    [Range(0, 999999)]
    public decimal? MinSalary { get; set; }

    [Range(0, 999999)]
    public decimal? MaxSalary { get; set; }

    [StringLength(200)]
    public string? Location { get; set; }

    public bool IsRemoteWork { get; set; } = false;

    [Range(1, 100)]
    public int? OpenPositions { get; set; } = 1;

    [Required]
    public DateTime PublishDate { get; set; } = DateTime.Now;

    public DateTime? ExpiryDate { get; set; }

    public DateTime? LastApplicationDate { get; set; }

    [StringLength(500)]
    public string? ContactInfo { get; set; }

    public int? CreatedById { get; set; }

    public int? UpdatedById { get; set; }

    // SEO ve Web için
    [StringLength(200)]
    public string? Slug { get; set; }

    [StringLength(500)]
    public string? MetaDescription { get; set; }

    [StringLength(200)]
    public string? Tags { get; set; }

    public int ViewCount { get; set; } = 0;

    public int ApplicationCount { get; set; } = 0;

    // Navigation Properties
    public virtual Position Position { get; set; } = null!;
    public virtual Department Department { get; set; } = null!;
    public virtual Person? CreatedBy { get; set; }
    public virtual Person? UpdatedBy { get; set; }
    public virtual ICollection<JobApplication> Applications { get; set; } = new List<JobApplication>();

    // Computed Properties
    public string StatusText => Status switch
    {
        JobPostingStatus.Draft => "Taslak",
        JobPostingStatus.Active => "Aktif",
        JobPostingStatus.Suspended => "Durduruldu",
        JobPostingStatus.Closed => "Kapatıldı",
        JobPostingStatus.Expired => "Süresi Doldu",
        _ => "Bilinmiyor"
    };

    public string StatusClass => Status switch
    {
        JobPostingStatus.Draft => "secondary",
        JobPostingStatus.Active => "success",
        JobPostingStatus.Suspended => "warning",
        JobPostingStatus.Closed => "danger",
        JobPostingStatus.Expired => "secondary",
        _ => "light"
    };

    public string EmploymentTypeText => EmploymentType switch
    {
        EmploymentType.FullTime => "Tam Zamanlı",
        EmploymentType.PartTime => "Yarı Zamanlı",
        EmploymentType.Contract => "Sözleşmeli",
        EmploymentType.Internship => "Staj",
        EmploymentType.Freelance => "Serbest Çalışan",
        _ => "Bilinmiyor"
    };

    public bool IsExpired => ExpiryDate.HasValue && ExpiryDate.Value < DateTime.Now;
    public bool IsApplicationDeadlinePassed => LastApplicationDate.HasValue && LastApplicationDate.Value < DateTime.Now;
    public new bool IsActive => Status == JobPostingStatus.Active && !IsExpired && !IsApplicationDeadlinePassed;
    
    public string SalaryRange
    {
        get
        {
            if (MinSalary.HasValue && MaxSalary.HasValue)
                return $"{MinSalary:N0} - {MaxSalary:N0} TL";
            else if (MinSalary.HasValue)
                return $"{MinSalary:N0}+ TL";
            else if (MaxSalary.HasValue)
                return $"Max {MaxSalary:N0} TL";
            else
                return "Görüşülür";
        }
    }

    public string ExperienceRange
    {
        get
        {
            if (MinExperience.HasValue && MaxExperience.HasValue)
                return $"{MinExperience}-{MaxExperience} yıl";
            else if (MinExperience.HasValue)
                return $"{MinExperience}+ yıl";
            else if (MaxExperience.HasValue)
                return $"Max {MaxExperience} yıl";
            else
                return "Deneyim seviyesi belirtilmemiş";
        }
    }

    public int DaysUntilExpiry
    {
        get
        {
            if (!ExpiryDate.HasValue) return int.MaxValue;
            return (int)(ExpiryDate.Value - DateTime.Now).TotalDays;
        }
    }

    public int DaysUntilApplicationDeadline
    {
        get
        {
            if (!LastApplicationDate.HasValue) return int.MaxValue;
            return (int)(LastApplicationDate.Value - DateTime.Now).TotalDays;
        }
    }
}
