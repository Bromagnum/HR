using System.ComponentModel.DataAnnotations;

namespace BLL.DTOs;

public class QualificationListDto
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
}

public class QualificationDetailDto
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
}

public class QualificationCreateDto
{
    [Required(ErrorMessage = "Yeterlilik adı zorunludur")]
    [StringLength(200, ErrorMessage = "Yeterlilik adı en fazla 200 karakter olabilir")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Kategori zorunludur")]
    [StringLength(100, ErrorMessage = "Kategori en fazla 100 karakter olabilir")]
    public string Category { get; set; } = string.Empty;

    [Required(ErrorMessage = "Veren kurum zorunludur")]
    [StringLength(200, ErrorMessage = "Veren kurum en fazla 200 karakter olabilir")]
    public string IssuingAuthority { get; set; } = string.Empty;

    [StringLength(100, ErrorMessage = "Kimlik numarası en fazla 100 karakter olabilir")]
    public string? CredentialNumber { get; set; }

    [Required(ErrorMessage = "Veriliş tarihi zorunludur")]
    public DateTime IssueDate { get; set; } = DateTime.Now;

    public DateTime? ExpirationDate { get; set; }

    public bool HasExpiration { get; set; } = false;

    [StringLength(100, ErrorMessage = "Seviye en fazla 100 karakter olabilir")]
    public string? Level { get; set; }

    [Range(0, 100, ErrorMessage = "Puan 0-100 arasında olmalıdır")]
    public int? Score { get; set; }

    [StringLength(1000, ErrorMessage = "Açıklama en fazla 1000 karakter olabilir")]
    public string? Description { get; set; }

    [StringLength(200, ErrorMessage = "Lokasyon en fazla 200 karakter olabilir")]
    public string? Location { get; set; }

    [Required(ErrorMessage = "Personel seçimi zorunludur")]
    public int PersonId { get; set; }
}

public class QualificationUpdateDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Yeterlilik adı zorunludur")]
    [StringLength(200, ErrorMessage = "Yeterlilik adı en fazla 200 karakter olabilir")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Kategori zorunludur")]
    [StringLength(100, ErrorMessage = "Kategori en fazla 100 karakter olabilir")]
    public string Category { get; set; } = string.Empty;

    [Required(ErrorMessage = "Veren kurum zorunludur")]
    [StringLength(200, ErrorMessage = "Veren kurum en fazla 200 karakter olabilir")]
    public string IssuingAuthority { get; set; } = string.Empty;

    [StringLength(100, ErrorMessage = "Kimlik numarası en fazla 100 karakter olabilir")]
    public string? CredentialNumber { get; set; }

    [Required(ErrorMessage = "Veriliş tarihi zorunludur")]
    public DateTime IssueDate { get; set; }

    public DateTime? ExpirationDate { get; set; }

    public bool HasExpiration { get; set; } = false;

    [StringLength(100, ErrorMessage = "Seviye en fazla 100 karakter olabilir")]
    public string? Level { get; set; }

    [Range(0, 100, ErrorMessage = "Puan 0-100 arasında olmalıdır")]
    public int? Score { get; set; }

    [StringLength(1000, ErrorMessage = "Açıklama en fazla 1000 karakter olabilir")]
    public string? Description { get; set; }

    [StringLength(200, ErrorMessage = "Lokasyon en fazla 200 karakter olabilir")]
    public string? Location { get; set; }

    [Required(ErrorMessage = "Personel seçimi zorunludur")]
    public int PersonId { get; set; }
}
