using System.ComponentModel.DataAnnotations;

namespace BLL.DTOs;

// List DTO - Candidate listesi için
public class CandidateListDto
{
    public int Id { get; set; }
    public string TcKimlikNo { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? City { get; set; }
    public int? ExperienceYears { get; set; }
    public string? CurrentCompany { get; set; }
    public string? CurrentPosition { get; set; }
    public decimal? ExpectedSalary { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? LastContactDate { get; set; }
    public int JobApplicationCount { get; set; }
    public bool IsActive { get; set; }
}

// Detail DTO - Candidate detayları için
public class CandidateDetailDto
{
    public int Id { get; set; }
    public string TcKimlikNo { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string? FatherName { get; set; }
    public string? MotherName { get; set; }
    public string? BirthPlace { get; set; }
    public DateTime? BirthDate { get; set; }
    public int Age { get; set; }
    
    // İletişim
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    
    // Kişisel
    public string? Gender { get; set; }
    public string? MaritalStatus { get; set; }
    public string? MilitaryStatus { get; set; }
    public string? DriverLicenseClass { get; set; }
    
    // Profesyonel
    public int? ExperienceYears { get; set; }
    public string? CurrentCompany { get; set; }
    public string? CurrentPosition { get; set; }
    public decimal? ExpectedSalary { get; set; }
    public decimal? CurrentSalary { get; set; }
    public DateTime? AvailableStartDate { get; set; }
    public string? PreferredWorkType { get; set; }
    
    // CV
    public string? CvFilePath { get; set; }
    public string? CvFileName { get; set; }
    public DateTime? CvUploadDate { get; set; }
    
    // Sosyal Medya
    public string? LinkedInUrl { get; set; }
    public string? GitHubUrl { get; set; }
    public string? PersonalWebsite { get; set; }
    
    // Durum
    public string Status { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public DateTime? LastContactDate { get; set; }
    public string? Source { get; set; }
    
    // Navigation Properties
    public List<CandidateEducationDto> Educations { get; set; } = new();
    public List<CandidateExperienceDto> Experiences { get; set; } = new();
    public List<CandidateSkillDto> Skills { get; set; } = new();
    public List<JobApplicationListDto> JobApplications { get; set; } = new();
    
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; }
}

// Create DTO - Yeni candidate oluşturma için
public class CandidateCreateDto
{
    [Required(ErrorMessage = "TC Kimlik No zorunludur")]
    [StringLength(11, MinimumLength = 11, ErrorMessage = "TC Kimlik No 11 karakter olmalıdır")]
    public string TcKimlikNo { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Ad zorunludur")]
    [StringLength(50, ErrorMessage = "Ad en fazla 50 karakter olabilir")]
    public string FirstName { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Soyad zorunludur")]
    [StringLength(50, ErrorMessage = "Soyad en fazla 50 karakter olabilir")]
    public string LastName { get; set; } = string.Empty;
    
    [StringLength(50)]
    public string? FatherName { get; set; }
    
    [StringLength(50)]
    public string? MotherName { get; set; }
    
    [StringLength(100)]
    public string? BirthPlace { get; set; }
    
    public DateTime? BirthDate { get; set; }
    
    [Required(ErrorMessage = "Email zorunludur")]
    [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz")]
    [StringLength(100)]
    public string Email { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Telefon zorunludur")]
    [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz")]
    [StringLength(20)]
    public string Phone { get; set; } = string.Empty;
    
    [StringLength(500)]
    public string? Address { get; set; }
    
    [StringLength(100)]
    public string? City { get; set; }
    
    [StringLength(100)]
    public string? Country { get; set; } = "Türkiye";
    
    [StringLength(20)]
    public string? Gender { get; set; }
    
    [StringLength(20)]
    public string? MaritalStatus { get; set; }
    
    [StringLength(20)]
    public string? MilitaryStatus { get; set; }
    
    [StringLength(10)]
    public string? DriverLicenseClass { get; set; }
    
    [Range(0, 50, ErrorMessage = "Deneyim yılı 0-50 arasında olmalıdır")]
    public int? ExperienceYears { get; set; }
    
    [StringLength(100)]
    public string? CurrentCompany { get; set; }
    
    [StringLength(100)]
    public string? CurrentPosition { get; set; }
    
    [Range(0, double.MaxValue, ErrorMessage = "Beklenen maaş pozitif olmalıdır")]
    public decimal? ExpectedSalary { get; set; }
    
    [Range(0, double.MaxValue, ErrorMessage = "Mevcut maaş pozitif olmalıdır")]
    public decimal? CurrentSalary { get; set; }
    
    public DateTime? AvailableStartDate { get; set; }
    
    [StringLength(20)]
    public string? PreferredWorkType { get; set; }
    
    [StringLength(200)]
    public string? LinkedInUrl { get; set; }
    
    [StringLength(200)]
    public string? GitHubUrl { get; set; }
    
    [StringLength(200)]
    public string? PersonalWebsite { get; set; }
    
    [StringLength(500)]
    public string? Notes { get; set; }
    
    [StringLength(100)]
    public string? Source { get; set; }
}

// Update DTO - Candidate güncelleme için
public class CandidateUpdateDto : CandidateCreateDto
{
    public int Id { get; set; }
    
    [StringLength(20)]
    public string Status { get; set; } = "Active";
    
    public DateTime? LastContactDate { get; set; }
}

// Filter DTO - Candidate filtreleme için
public class CandidateFilterDto
{
    public string? SearchTerm { get; set; }
    public string? Status { get; set; }
    public string? City { get; set; }
    public int? MinExperienceYears { get; set; }
    public int? MaxExperienceYears { get; set; }
    public decimal? MinExpectedSalary { get; set; }
    public decimal? MaxExpectedSalary { get; set; }
    public string? SkillName { get; set; }
    public string? Source { get; set; }
    public DateTime? CreatedAfter { get; set; }
    public DateTime? CreatedBefore { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string SortBy { get; set; } = "CreatedAt";
    public bool SortDescending { get; set; } = true;
}

// Search Result DTO
public class CandidateSearchResultDto
{
    public List<CandidateListDto> Candidates { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public bool HasPreviousPage { get; set; }
    public bool HasNextPage { get; set; }
}
