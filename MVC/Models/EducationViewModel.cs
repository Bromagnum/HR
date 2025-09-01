using System.ComponentModel.DataAnnotations;

namespace MVC.Models;

public class EducationListViewModel
{
    public int Id { get; set; }
    
    [Display(Name = "Okul Adı")]
    public string SchoolName { get; set; } = string.Empty;
    
    [Display(Name = "Derece")]
    public string Degree { get; set; } = string.Empty;
    
    [Display(Name = "Bölüm")]
    public string FieldOfStudy { get; set; } = string.Empty;
    
    [Display(Name = "Başlangıç Tarihi")]
    [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = false)]
    public DateTime StartDate { get; set; }
    
    [Display(Name = "Bitiş Tarihi")]
    [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = false)]
    public DateTime? EndDate { get; set; }
    
    [Display(Name = "Devam Ediyor")]
    public bool IsOngoing { get; set; }
    
    [Display(Name = "GPA")]
    [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
    public decimal? GPA { get; set; }
    
    [Display(Name = "Lokasyon")]
    public string? Location { get; set; }
    
    [Display(Name = "Personel")]
    public string PersonName { get; set; } = string.Empty;
    
    [Display(Name = "Sicil No")]
    public string? PersonEmployeeNumber { get; set; }
    
    public int PersonId { get; set; }
    
    [Display(Name = "Oluşturma Tarihi")]
    [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}", ApplyFormatInEditMode = false)]
    public DateTime CreatedAt { get; set; }
    
    [Display(Name = "Durum")]
    public bool IsActive { get; set; }
}

public class EducationDetailViewModel
{
    public int Id { get; set; }
    
    [Display(Name = "Okul Adı")]
    public string SchoolName { get; set; } = string.Empty;
    
    [Display(Name = "Derece")]
    public string Degree { get; set; } = string.Empty;
    
    [Display(Name = "Bölüm")]
    public string FieldOfStudy { get; set; } = string.Empty;
    
    [Display(Name = "Başlangıç Tarihi")]
    [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = false)]
    public DateTime StartDate { get; set; }
    
    [Display(Name = "Bitiş Tarihi")]
    [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = false)]
    public DateTime? EndDate { get; set; }
    
    [Display(Name = "Devam Ediyor")]
    public bool IsOngoing { get; set; }
    
    [Display(Name = "GPA")]
    [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
    public decimal? GPA { get; set; }
    
    [Display(Name = "Açıklama")]
    public string? Description { get; set; }
    
    [Display(Name = "Lokasyon")]
    public string? Location { get; set; }
    
    public int PersonId { get; set; }
    
    [Display(Name = "Personel")]
    public string PersonName { get; set; } = string.Empty;
    
    [Display(Name = "Sicil No")]
    public string? PersonEmployeeNumber { get; set; }
    
    [Display(Name = "E-posta")]
    public string? PersonEmail { get; set; }
    
    [Display(Name = "Telefon")]
    public string? PersonPhone { get; set; }
    
    [Display(Name = "Departman")]
    public string? PersonDepartmentName { get; set; }
    
    [Display(Name = "Oluşturma Tarihi")]
    [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}", ApplyFormatInEditMode = false)]
    public DateTime CreatedAt { get; set; }
    
    [Display(Name = "Güncelleme Tarihi")]
    [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}", ApplyFormatInEditMode = false)]
    public DateTime? UpdatedAt { get; set; }
    
    [Display(Name = "Durum")]
    public bool IsActive { get; set; }
}

public class EducationCreateViewModel
{
    [Required(ErrorMessage = "Okul adı zorunludur")]
    [StringLength(100, ErrorMessage = "Okul adı en fazla 100 karakter olabilir")]
    [Display(Name = "Okul Adı")]
    public string SchoolName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Derece zorunludur")]
    [StringLength(100, ErrorMessage = "Derece en fazla 100 karakter olabilir")]
    [Display(Name = "Derece")]
    public string Degree { get; set; } = string.Empty;

    [Required(ErrorMessage = "Bölüm zorunludur")]
    [StringLength(100, ErrorMessage = "Bölüm en fazla 100 karakter olabilir")]
    [Display(Name = "Bölüm/Alan")]
    public string FieldOfStudy { get; set; } = string.Empty;

    [Required(ErrorMessage = "Başlangıç tarihi zorunludur")]
    [Display(Name = "Başlangıç Tarihi")]
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; } = DateTime.Now.AddYears(-4);

    [Display(Name = "Bitiş Tarihi")]
    [DataType(DataType.Date)]
    public DateTime? EndDate { get; set; }

    [Display(Name = "Devam Ediyor")]
    public bool IsOngoing { get; set; } = false;

    [Range(0, 4, ErrorMessage = "GPA 0-4 arasında olmalıdır")]
    [Display(Name = "GPA (Not Ortalaması)")]
    [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
    public decimal? GPA { get; set; }

    [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir")]
    [Display(Name = "Açıklama")]
    [DataType(DataType.MultilineText)]
    public string? Description { get; set; }

    [StringLength(100, ErrorMessage = "Lokasyon en fazla 100 karakter olabilir")]
    [Display(Name = "Lokasyon/Şehir")]
    public string? Location { get; set; }

    [Required(ErrorMessage = "Personel seçimi zorunludur")]
    [Display(Name = "Personel")]
    public int PersonId { get; set; }
}

public class EducationEditViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Okul adı zorunludur")]
    [StringLength(100, ErrorMessage = "Okul adı en fazla 100 karakter olabilir")]
    [Display(Name = "Okul Adı")]
    public string SchoolName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Derece zorunludur")]
    [StringLength(100, ErrorMessage = "Derece en fazla 100 karakter olabilir")]
    [Display(Name = "Derece")]
    public string Degree { get; set; } = string.Empty;

    [Required(ErrorMessage = "Bölüm zorunludur")]
    [StringLength(100, ErrorMessage = "Bölüm en fazla 100 karakter olabilir")]
    [Display(Name = "Bölüm/Alan")]
    public string FieldOfStudy { get; set; } = string.Empty;

    [Required(ErrorMessage = "Başlangıç tarihi zorunludur")]
    [Display(Name = "Başlangıç Tarihi")]
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }

    [Display(Name = "Bitiş Tarihi")]
    [DataType(DataType.Date)]
    public DateTime? EndDate { get; set; }

    [Display(Name = "Devam Ediyor")]
    public bool IsOngoing { get; set; } = false;

    [Range(0, 4, ErrorMessage = "GPA 0-4 arasında olmalıdır")]
    [Display(Name = "GPA (Not Ortalaması)")]
    [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
    public decimal? GPA { get; set; }

    [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir")]
    [Display(Name = "Açıklama")]
    [DataType(DataType.MultilineText)]
    public string? Description { get; set; }

    [StringLength(100, ErrorMessage = "Lokasyon en fazla 100 karakter olabilir")]
    [Display(Name = "Lokasyon/Şehir")]
    public string? Location { get; set; }

    [Required(ErrorMessage = "Personel seçimi zorunludur")]
    [Display(Name = "Personel")]
    public int PersonId { get; set; }
}

