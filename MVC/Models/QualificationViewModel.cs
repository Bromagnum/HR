using System.ComponentModel.DataAnnotations;

namespace MVC.Models;

public class QualificationIndexViewModel
{
    public IEnumerable<QualificationListViewModel> Qualifications { get; set; } = new List<QualificationListViewModel>();
    public string? SearchTerm { get; set; }
    public string? SelectedCategory { get; set; }
    public bool ShowExpiredOnly { get; set; }
    public bool ShowExpiringSoon { get; set; }
}

public class QualificationListViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string IssuingAuthority { get; set; } = string.Empty;
    public string? CredentialNumber { get; set; }
    public DateTime IssueDate { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public bool HasExpiration { get; set; }
    public string? Level { get; set; }
    public int? Score { get; set; }
    public string? Location { get; set; }
    public bool IsActive { get; set; }
    
    // Person Info
    public int PersonId { get; set; }
    public string PersonName { get; set; } = string.Empty;
    public string? DepartmentName { get; set; }
    
    // Computed Properties
    public bool IsExpired => HasExpiration && ExpirationDate.HasValue && ExpirationDate.Value < DateTime.Now;
    public bool IsExpiringSoon => HasExpiration && ExpirationDate.HasValue && 
                                  ExpirationDate.Value > DateTime.Now && 
                                  ExpirationDate.Value <= DateTime.Now.AddDays(30);
    public string ExpirationStatus
    {
        get
        {
            if (!HasExpiration) return "Süresi Yok";
            if (IsExpired) return "Süresi Dolmuş";
            if (IsExpiringSoon) return "Süresi Yaklaşıyor";
            return "Geçerli";
        }
    }
    
    public string ExpirationBadgeClass
    {
        get
        {
            if (!HasExpiration) return "bg-info";
            if (IsExpired) return "bg-danger";
            if (IsExpiringSoon) return "bg-warning";
            return "bg-success";
        }
    }
}

public class QualificationDetailViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string IssuingAuthority { get; set; } = string.Empty;
    public string? CredentialNumber { get; set; }
    public DateTime IssueDate { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public bool HasExpiration { get; set; }
    public string? Level { get; set; }
    public int? Score { get; set; }
    public string? Description { get; set; }
    public string? AttachmentPath { get; set; }
    public string? Location { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    // Person Info
    public int PersonId { get; set; }
    public string PersonName { get; set; } = string.Empty;
    public string? DepartmentName { get; set; }
    
    // Computed Properties
    public bool IsExpired => HasExpiration && ExpirationDate.HasValue && ExpirationDate.Value < DateTime.Now;
    public bool IsExpiringSoon => HasExpiration && ExpirationDate.HasValue && 
                                  ExpirationDate.Value > DateTime.Now && 
                                  ExpirationDate.Value <= DateTime.Now.AddDays(30);
    public string ExpirationStatus
    {
        get
        {
            if (!HasExpiration) return "Süresi Yok";
            if (IsExpired) return "Süresi Dolmuş";
            if (IsExpiringSoon) return "Süresi Yaklaşıyor";
            return "Geçerli";
        }
    }
}

public class QualificationCreateViewModel
{
    [Required(ErrorMessage = "Yeterlilik adı zorunludur.")]
    [StringLength(200, ErrorMessage = "Yeterlilik adı en fazla 200 karakter olabilir.")]
    [Display(Name = "Yeterlilik Adı")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Kategori zorunludur.")]
    [StringLength(100, ErrorMessage = "Kategori en fazla 100 karakter olabilir.")]
    [Display(Name = "Kategori")]
    public string Category { get; set; } = string.Empty;

    [Required(ErrorMessage = "Veren kurum zorunludur.")]
    [StringLength(200, ErrorMessage = "Veren kurum en fazla 200 karakter olabilir.")]
    [Display(Name = "Veren Kurum")]
    public string IssuingAuthority { get; set; } = string.Empty;

    [StringLength(100, ErrorMessage = "Kimlik numarası en fazla 100 karakter olabilir.")]
    [Display(Name = "Kimlik/Sertifika Numarası")]
    public string? CredentialNumber { get; set; }

    [Required(ErrorMessage = "Veriliş tarihi zorunludur.")]
    [DataType(DataType.Date)]
    [Display(Name = "Veriliş Tarihi")]
    public DateTime IssueDate { get; set; } = DateTime.Now.Date;

    [DataType(DataType.Date)]
    [Display(Name = "Son Geçerlilik Tarihi")]
    public DateTime? ExpirationDate { get; set; }

    [Display(Name = "Süre Sınırı Var")]
    public bool HasExpiration { get; set; } = false;

    [StringLength(100, ErrorMessage = "Seviye en fazla 100 karakter olabilir.")]
    [Display(Name = "Seviye")]
    public string? Level { get; set; }

    [Range(0, 100, ErrorMessage = "Puan 0-100 arasında olmalıdır.")]
    [Display(Name = "Puan")]
    public int? Score { get; set; }

    [StringLength(1000, ErrorMessage = "Açıklama en fazla 1000 karakter olabilir.")]
    [Display(Name = "Açıklama")]
    public string? Description { get; set; }

    [StringLength(200, ErrorMessage = "Lokasyon en fazla 200 karakter olabilir.")]
    [Display(Name = "Lokasyon")]
    public string? Location { get; set; }

    [Required(ErrorMessage = "Personel seçimi zorunludur.")]
    [Display(Name = "Personel")]
    public int PersonId { get; set; }
}

public class QualificationEditViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Yeterlilik adı zorunludur.")]
    [StringLength(200, ErrorMessage = "Yeterlilik adı en fazla 200 karakter olabilir.")]
    [Display(Name = "Yeterlilik Adı")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Kategori zorunludur.")]
    [StringLength(100, ErrorMessage = "Kategori en fazla 100 karakter olabilir.")]
    [Display(Name = "Kategori")]
    public string Category { get; set; } = string.Empty;

    [Required(ErrorMessage = "Veren kurum zorunludur.")]
    [StringLength(200, ErrorMessage = "Veren kurum en fazla 200 karakter olabilir.")]
    [Display(Name = "Veren Kurum")]
    public string IssuingAuthority { get; set; } = string.Empty;

    [StringLength(100, ErrorMessage = "Kimlik numarası en fazla 100 karakter olabilir.")]
    [Display(Name = "Kimlik/Sertifika Numarası")]
    public string? CredentialNumber { get; set; }

    [Required(ErrorMessage = "Veriliş tarihi zorunludur.")]
    [DataType(DataType.Date)]
    [Display(Name = "Veriliş Tarihi")]
    public DateTime IssueDate { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Son Geçerlilik Tarihi")]
    public DateTime? ExpirationDate { get; set; }

    [Display(Name = "Süre Sınırı Var")]
    public bool HasExpiration { get; set; } = false;

    [StringLength(100, ErrorMessage = "Seviye en fazla 100 karakter olabilir.")]
    [Display(Name = "Seviye")]
    public string? Level { get; set; }

    [Range(0, 100, ErrorMessage = "Puan 0-100 arasında olmalıdır.")]
    [Display(Name = "Puan")]
    public int? Score { get; set; }

    [StringLength(1000, ErrorMessage = "Açıklama en fazla 1000 karakter olabilir.")]
    [Display(Name = "Açıklama")]
    public string? Description { get; set; }

    [StringLength(200, ErrorMessage = "Lokasyon en fazla 200 karakter olabilir.")]
    [Display(Name = "Lokasyon")]
    public string? Location { get; set; }

    [Required(ErrorMessage = "Personel seçimi zorunludur.")]
    [Display(Name = "Personel")]
    public int PersonId { get; set; }
}
