using System.ComponentModel.DataAnnotations;

namespace BLL.DTOs;

public class EducationListDto
{
    public int Id { get; set; }
    public string SchoolName { get; set; } = string.Empty;
    public string Degree { get; set; } = string.Empty;
    public string FieldOfStudy { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsOngoing { get; set; }
    public decimal? GPA { get; set; }
    public string? Location { get; set; }
    public string PersonName { get; set; } = string.Empty;
    public string? PersonEmployeeNumber { get; set; }
    public int PersonId { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }
}

public class EducationDetailDto
{
    public int Id { get; set; }
    public string SchoolName { get; set; } = string.Empty;
    public string Degree { get; set; } = string.Empty;
    public string FieldOfStudy { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsOngoing { get; set; }
    public decimal? GPA { get; set; }
    public string? Description { get; set; }
    public string? Location { get; set; }
    public int PersonId { get; set; }
    public string PersonName { get; set; } = string.Empty;
    public string? PersonEmployeeNumber { get; set; }
    public string? PersonEmail { get; set; }
    public string? PersonPhone { get; set; }
    public string? PersonDepartmentName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; }
}

public class EducationCreateDto
{
    [Required(ErrorMessage = "Okul adı zorunludur")]
    [StringLength(100, ErrorMessage = "Okul adı en fazla 100 karakter olabilir")]
    public string SchoolName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Derece zorunludur")]
    [StringLength(100, ErrorMessage = "Derece en fazla 100 karakter olabilir")]
    public string Degree { get; set; } = string.Empty;


    [Required(ErrorMessage = "Bölüm zorunludur")]
    [StringLength(100, ErrorMessage = "Bölüm en fazla 100 karakter olabilir")]
    public string FieldOfStudy { get; set; } = string.Empty;

    [Required(ErrorMessage = "Başlangıç tarihi zorunludur")]
    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public bool IsOngoing { get; set; } = false;

    [Range(0, 4, ErrorMessage = "GPA 0-4 arasında olmalıdır")]
    public decimal? GPA { get; set; }

    [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir")]
    public string? Description { get; set; }

    [StringLength(100, ErrorMessage = "Lokasyon en fazla 100 karakter olabilir")]
    public string? Location { get; set; }

    [Required(ErrorMessage = "Personel seçimi zorunludur")]
    public int PersonId { get; set; }
}

public class EducationUpdateDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Okul adı zorunludur")]
    [StringLength(100, ErrorMessage = "Okul adı en fazla 100 karakter olabilir")]
    public string SchoolName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Derece zorunludur")]
    [StringLength(100, ErrorMessage = "Derece en fazla 100 karakter olabilir")]
    public string Degree { get; set; } = string.Empty;

    [Required(ErrorMessage = "Bölüm zorunludur")]
    [StringLength(100, ErrorMessage = "Bölüm en fazla 100 karakter olabilir")]
    public string FieldOfStudy { get; set; } = string.Empty;

    [Required(ErrorMessage = "Başlangıç tarihi zorunludur")]
    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public bool IsOngoing { get; set; } = false;

    [Range(0, 4, ErrorMessage = "GPA 0-4 arasında olmalıdır")]
    public decimal? GPA { get; set; }

    [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir")]
    public string? Description { get; set; }

    [StringLength(100, ErrorMessage = "Lokasyon en fazla 100 karakter olabilir")]
    public string? Location { get; set; }

    [Required(ErrorMessage = "Personel seçimi zorunludur")]
    public int PersonId { get; set; }
}

