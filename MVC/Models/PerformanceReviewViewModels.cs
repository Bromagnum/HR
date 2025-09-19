using System.ComponentModel.DataAnnotations;
using DAL.Entities;

namespace MVC.Models;

public class PerformanceReviewCreateViewModel
{
    [Required(ErrorMessage = "Personel seçimi zorunludur")]
    [Display(Name = "Personel")]
    public int PersonId { get; set; }

    [Required(ErrorMessage = "Değerlendirme dönemi seçimi zorunludur")]
    [Display(Name = "Değerlendirme Dönemi")]
    public int ReviewPeriodId { get; set; }

    [Required(ErrorMessage = "Değerlendiren seçimi zorunludur")]
    [Display(Name = "Değerlendiren")]
    public int ReviewerId { get; set; }

    [Range(1, 5, ErrorMessage = "Genel Skor 1-5 arasında olmalıdır")]
    [Display(Name = "Genel Skor")]
    public int OverallScore { get; set; } = 3;

    [Range(1, 5, ErrorMessage = "İş Kalitesi Skoru 1-5 arasında olmalıdır")]
    [Display(Name = "İş Kalitesi")]
    public int JobQualityScore { get; set; } = 3;

    [Range(1, 5, ErrorMessage = "Üretkenlik Skoru 1-5 arasında olmalıdır")]
    [Display(Name = "Üretkenlik")]
    public int ProductivityScore { get; set; } = 3;

    [Range(1, 5, ErrorMessage = "Ekip Çalışması Skoru 1-5 arasında olmalıdır")]
    [Display(Name = "Ekip Çalışması")]
    public int TeamworkScore { get; set; } = 3;

    [Range(1, 5, ErrorMessage = "İletişim Skoru 1-5 arasında olmalıdır")]
    [Display(Name = "İletişim")]
    public int CommunicationScore { get; set; } = 3;

    [Range(1, 5, ErrorMessage = "Liderlik Skoru 1-5 arasında olmalıdır")]
    [Display(Name = "Liderlik")]
    public int LeadershipScore { get; set; } = 3;

    [Range(1, 5, ErrorMessage = "İnisiyatif Skoru 1-5 arasında olmalıdır")]
    [Display(Name = "İnisiyatif")]
    public int InitiativeScore { get; set; } = 3;

    [Range(1, 5, ErrorMessage = "Problem Çözme Skoru 1-5 arasında olmalıdır")]
    [Display(Name = "Problem Çözme")]
    public int ProblemSolvingScore { get; set; } = 3;

    [Range(1, 5, ErrorMessage = "Uyum Skoru 1-5 arasında olmalıdır")]
    [Display(Name = "Uyum")]
    public int AdaptabilityScore { get; set; } = 3;

    [StringLength(2000, ErrorMessage = "Güçlü yönler en fazla 2000 karakter olabilir")]
    [Display(Name = "Güçlü Yönler")]
    public string? Strengths { get; set; }

    [StringLength(2000, ErrorMessage = "Gelişim alanları en fazla 2000 karakter olabilir")]
    [Display(Name = "Gelişim Alanları")]
    public string? AreasForImprovement { get; set; }

    [StringLength(2000, ErrorMessage = "Başarılar en fazla 2000 karakter olabilir")]
    [Display(Name = "Başarılar")]
    public string? Achievements { get; set; }

    [StringLength(2000, ErrorMessage = "Hedefler en fazla 2000 karakter olabilir")]
    [Display(Name = "Hedefler")]
    public string? Goals { get; set; }

    [StringLength(2000, ErrorMessage = "Değerlendiren yorumları en fazla 2000 karakter olabilir")]
    [Display(Name = "Değerlendiren Yorumları")]
    public string? ReviewerComments { get; set; }
}

public class PerformanceReviewEditViewModel
{
    public int Id { get; set; }

    [Range(1, 5, ErrorMessage = "Genel Skor 1-5 arasında olmalıdır")]
    [Display(Name = "Genel Skor")]
    public int OverallScore { get; set; }

    [Range(1, 5, ErrorMessage = "İş Kalitesi Skoru 1-5 arasında olmalıdır")]
    [Display(Name = "İş Kalitesi")]
    public int JobQualityScore { get; set; }

    [Range(1, 5, ErrorMessage = "Üretkenlik Skoru 1-5 arasında olmalıdır")]
    [Display(Name = "Üretkenlik")]
    public int ProductivityScore { get; set; }

    [Range(1, 5, ErrorMessage = "Ekip Çalışması Skoru 1-5 arasında olmalıdır")]
    [Display(Name = "Ekip Çalışması")]
    public int TeamworkScore { get; set; }

    [Range(1, 5, ErrorMessage = "İletişim Skoru 1-5 arasında olmalıdır")]
    [Display(Name = "İletişim")]
    public int CommunicationScore { get; set; }

    [Range(1, 5, ErrorMessage = "Liderlik Skoru 1-5 arasında olmalıdır")]
    [Display(Name = "Liderlik")]
    public int LeadershipScore { get; set; }

    [Range(1, 5, ErrorMessage = "İnisiyatif Skoru 1-5 arasında olmalıdır")]
    [Display(Name = "İnisiyatif")]
    public int InitiativeScore { get; set; }

    [Range(1, 5, ErrorMessage = "Problem Çözme Skoru 1-5 arasında olmalıdır")]
    [Display(Name = "Problem Çözme")]
    public int ProblemSolvingScore { get; set; }

    [Range(1, 5, ErrorMessage = "Uyum Skoru 1-5 arasında olmalıdır")]
    [Display(Name = "Uyum")]
    public int AdaptabilityScore { get; set; }

    [StringLength(2000, ErrorMessage = "Güçlü yönler en fazla 2000 karakter olabilir")]
    [Display(Name = "Güçlü Yönler")]
    public string? Strengths { get; set; }

    [StringLength(2000, ErrorMessage = "Gelişim alanları en fazla 2000 karakter olabilir")]
    [Display(Name = "Gelişim Alanları")]
    public string? AreasForImprovement { get; set; }

    [StringLength(2000, ErrorMessage = "Başarılar en fazla 2000 karakter olabilir")]
    [Display(Name = "Başarılar")]
    public string? Achievements { get; set; }

    [StringLength(2000, ErrorMessage = "Hedefler en fazla 2000 karakter olabilir")]
    [Display(Name = "Hedefler")]
    public string? Goals { get; set; }

    [StringLength(2000, ErrorMessage = "Değerlendiren yorumları en fazla 2000 karakter olabilir")]
    [Display(Name = "Değerlendiren Yorumları")]
    public string? ReviewerComments { get; set; }

    // Read-only properties for display
    public string PersonName { get; set; } = string.Empty;
    public string ReviewPeriodName { get; set; } = string.Empty;
    public string ReviewerName { get; set; } = string.Empty;
    public ReviewStatus Status { get; set; }
    public string StatusText { get; set; } = string.Empty;
}

public class SelfAssessmentViewModel
{
    public int PerformanceReviewId { get; set; }

    [Range(1, 5, ErrorMessage = "Öz değerlendirme skoru 1-5 arasında olmalıdır")]
    [Display(Name = "Öz Değerlendirme Skoru")]
    public int SelfOverallScore { get; set; } = 3;

    [StringLength(2000, ErrorMessage = "Öz değerlendirme yorumları en fazla 2000 karakter olabilir")]
    [Display(Name = "Öz Değerlendirme Yorumları")]
    public string? SelfAssessmentComments { get; set; }

    [StringLength(2000, ErrorMessage = "Çalışan yorumları en fazla 2000 karakter olabilir")]
    [Display(Name = "Ek Yorumlar")]
    public string? EmployeeComments { get; set; }

    // Read-only properties for display
    public string PersonName { get; set; } = string.Empty;
    public string ReviewPeriodName { get; set; } = string.Empty;
}

public class ReviewPeriodCreateViewModel
{
    [Required(ErrorMessage = "Dönem adı zorunludur")]
    [StringLength(100, ErrorMessage = "Dönem adı en fazla 100 karakter olabilir")]
    [Display(Name = "Dönem Adı")]
    public string Name { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir")]
    [Display(Name = "Açıklama")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Dönem başlangıç tarihi zorunludur")]
    [Display(Name = "Dönem Başlangıç Tarihi")]
    public DateTime StartDate { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "Dönem bitiş tarihi zorunludur")]
    [Display(Name = "Dönem Bitiş Tarihi")]
    public DateTime EndDate { get; set; } = DateTime.Now.AddYears(1);

    [Required(ErrorMessage = "Değerlendirme başlangıç tarihi zorunludur")]
    [Display(Name = "Değerlendirme Başlangıç Tarihi")]
    public DateTime ReviewStartDate { get; set; } = DateTime.Now.AddYears(1);

    [Required(ErrorMessage = "Değerlendirme bitiş tarihi zorunludur")]
    [Display(Name = "Değerlendirme Bitiş Tarihi")]
    public DateTime ReviewEndDate { get; set; } = DateTime.Now.AddYears(1).AddMonths(1);

    [Required(ErrorMessage = "Dönem türü seçimi zorunludur")]
    [Display(Name = "Dönem Türü")]
    public ReviewPeriodType Type { get; set; } = ReviewPeriodType.Annual;

    [Display(Name = "Aktif")]
    public bool IsActive { get; set; } = true;
}

public class ReviewPeriodEditViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Dönem adı zorunludur")]
    [StringLength(100, ErrorMessage = "Dönem adı en fazla 100 karakter olabilir")]
    [Display(Name = "Dönem Adı")]
    public string Name { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir")]
    [Display(Name = "Açıklama")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Dönem başlangıç tarihi zorunludur")]
    [Display(Name = "Dönem Başlangıç Tarihi")]
    public DateTime StartDate { get; set; }

    [Required(ErrorMessage = "Dönem bitiş tarihi zorunludur")]
    [Display(Name = "Dönem Bitiş Tarihi")]
    public DateTime EndDate { get; set; }

    [Required(ErrorMessage = "Değerlendirme başlangıç tarihi zorunludur")]
    [Display(Name = "Değerlendirme Başlangıç Tarihi")]
    public DateTime ReviewStartDate { get; set; }

    [Required(ErrorMessage = "Değerlendirme bitiş tarihi zorunludur")]
    [Display(Name = "Değerlendirme Bitiş Tarihi")]
    public DateTime ReviewEndDate { get; set; }

    [Required(ErrorMessage = "Dönem türü seçimi zorunludur")]
    [Display(Name = "Dönem Türü")]
    public ReviewPeriodType Type { get; set; }

    [Display(Name = "Aktif")]
    public bool IsActive { get; set; }
}

public class PerformanceGoalCreateViewModel
{
    [Required(ErrorMessage = "Hedef başlığı zorunludur")]
    [StringLength(200, ErrorMessage = "Hedef başlığı en fazla 200 karakter olabilir")]
    [Display(Name = "Hedef Başlığı")]
    public string Title { get; set; } = string.Empty;

    [StringLength(1000, ErrorMessage = "Hedef açıklaması en fazla 1000 karakter olabilir")]
    [Display(Name = "Açıklama")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Hedef tarihi zorunludur")]
    [Display(Name = "Hedef Tarihi")]
    public DateTime TargetDate { get; set; } = DateTime.Now.AddMonths(3);

    [Display(Name = "Öncelik")]
    public GoalPriority Priority { get; set; } = GoalPriority.Medium;

    [StringLength(1000, ErrorMessage = "Notlar en fazla 1000 karakter olabilir")]
    [Display(Name = "Notlar")]
    public string? Notes { get; set; }
}

public class PerformanceGoalEditViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Hedef başlığı zorunludur")]
    [StringLength(200, ErrorMessage = "Hedef başlığı en fazla 200 karakter olabilir")]
    [Display(Name = "Hedef Başlığı")]
    public string Title { get; set; } = string.Empty;

    [StringLength(1000, ErrorMessage = "Hedef açıklaması en fazla 1000 karakter olabilir")]
    [Display(Name = "Açıklama")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Hedef tarihi zorunludur")]
    [Display(Name = "Hedef Tarihi")]
    public DateTime TargetDate { get; set; }

    [Display(Name = "Durum")]
    public GoalStatus Status { get; set; }

    [Range(0, 100, ErrorMessage = "İlerleme yüzdesi 0-100 arasında olmalıdır")]
    [Display(Name = "İlerleme Yüzdesi")]
    public int ProgressPercentage { get; set; }

    [Display(Name = "Öncelik")]
    public GoalPriority Priority { get; set; }

    [StringLength(1000, ErrorMessage = "Notlar en fazla 1000 karakter olabilir")]
    [Display(Name = "Notlar")]
    public string? Notes { get; set; }
}
