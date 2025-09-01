using System.ComponentModel.DataAnnotations;

namespace MVC.Models;

public class PersonListViewModel
{
    public int Id { get; set; }
    
    [Display(Name = "TC Kimlik No")]
    public string TcKimlikNo { get; set; } = string.Empty;
    
    [Display(Name = "Ad")]
    public string FirstName { get; set; } = string.Empty;
    
    [Display(Name = "Soyad")]
    public string LastName { get; set; } = string.Empty;
    
    [Display(Name = "Ad Soyad")]
    public string FullName => $"{FirstName} {LastName}";
    
    [Display(Name = "E-posta")]
    public string? Email { get; set; }
    
    [Display(Name = "Telefon")]
    public string? Phone { get; set; }
    
    [Display(Name = "Personel No")]
    public string? EmployeeNumber { get; set; }
    
    [Display(Name = "Pozisyon")]
    public string? Position { get; set; }
    
    [Display(Name = "Departman")]
    public string? DepartmentName { get; set; }
    
    [Display(Name = "İşe Giriş Tarihi")]
    [DataType(DataType.Date)]
    public DateTime? HireDate { get; set; }
    
    [Display(Name = "Maaş")]
    [DataType(DataType.Currency)]
    public decimal? Salary { get; set; }
    
    [Display(Name = "Aktif")]
    public bool IsActive { get; set; }
}

public class PersonCreateViewModel
{
    [Required(ErrorMessage = "TC Kimlik No zorunludur")]
    [StringLength(11, MinimumLength = 11, ErrorMessage = "TC Kimlik No 11 haneli olmalıdır")]
    [Display(Name = "TC Kimlik No")]
    public string TcKimlikNo { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Ad zorunludur")]
    [StringLength(50, ErrorMessage = "Ad en fazla 50 karakter olabilir")]
    [Display(Name = "Ad")]
    public string FirstName { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Soyad zorunludur")]
    [StringLength(50, ErrorMessage = "Soyad en fazla 50 karakter olabilir")]
    [Display(Name = "Soyad")]
    public string LastName { get; set; } = string.Empty;
    
    [StringLength(50, ErrorMessage = "Baba adı en fazla 50 karakter olabilir")]
    [Display(Name = "Baba Adı")]
    public string? FatherName { get; set; }
    
    [StringLength(50, ErrorMessage = "Anne adı en fazla 50 karakter olabilir")]
    [Display(Name = "Anne Adı")]
    public string? MotherName { get; set; }
    
    [StringLength(100, ErrorMessage = "Doğum yeri en fazla 100 karakter olabilir")]
    [Display(Name = "Doğum Yeri")]
    public string? BirthPlace { get; set; }
    
    [DataType(DataType.Date)]
    [Display(Name = "Doğum Tarihi")]
    public DateTime? BirthDate { get; set; }
    
    [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz")]
    [StringLength(100, ErrorMessage = "E-posta en fazla 100 karakter olabilir")]
    [Display(Name = "E-posta")]
    public string? Email { get; set; }
    
    [StringLength(20, ErrorMessage = "Telefon en fazla 20 karakter olabilir")]
    [Display(Name = "Telefon")]
    public string? Phone { get; set; }
    
    [StringLength(500, ErrorMessage = "Adres en fazla 500 karakter olabilir")]
    [Display(Name = "Adres")]
    public string? Address { get; set; }
    
    [StringLength(20, ErrorMessage = "Personel no en fazla 20 karakter olabilir")]
    [Display(Name = "Personel No")]
    public string? EmployeeNumber { get; set; }
    
    [DataType(DataType.Date)]
    [Display(Name = "İşe Giriş Tarihi")]
    public DateTime? HireDate { get; set; }
    
    [Range(0, double.MaxValue, ErrorMessage = "Maaş 0'dan büyük olmalıdır")]
    [Display(Name = "Maaş")]
    public decimal? Salary { get; set; }
    
    [StringLength(100, ErrorMessage = "Pozisyon en fazla 100 karakter olabilir")]
    [Display(Name = "Pozisyon")]
    public string? Position { get; set; }
    
    [Display(Name = "Departman")]
    public int? DepartmentId { get; set; }
    
    [StringLength(20, ErrorMessage = "Medeni hal en fazla 20 karakter olabilir")]
    [Display(Name = "Medeni Hal")]
    public string? MaritalStatus { get; set; }
    
    [StringLength(5, ErrorMessage = "Kan grubu en fazla 5 karakter olabilir")]
    [Display(Name = "Kan Grubu")]
    public string? BloodType { get; set; }
    
    [StringLength(50, ErrorMessage = "Din en fazla 50 karakter olabilir")]
    [Display(Name = "Din")]
    public string? Religion { get; set; }
    
    [StringLength(50, ErrorMessage = "Askerlik durumu en fazla 50 karakter olabilir")]
    [Display(Name = "Askerlik Durumu")]
    public string? MilitaryStatus { get; set; }
    
    [StringLength(10, ErrorMessage = "Ehliyet sınıfı en fazla 10 karakter olabilir")]
    [Display(Name = "Ehliyet Sınıfı")]
    public string? DriverLicenseClass { get; set; }
    
    [StringLength(20, ErrorMessage = "SSK no en fazla 20 karakter olabilir")]
    [Display(Name = "SSK No")]
    public string? SskNumber { get; set; }
    
    [DataType(DataType.Date)]
    [Display(Name = "SSK Başlangıç Tarihi")]
    public DateTime? SskStartDate { get; set; }
}

public class PersonEditViewModel : PersonCreateViewModel
{
    public int Id { get; set; }
    
    [Display(Name = "Aktif")]
    public bool IsActive { get; set; } = true;
}

public class PersonDetailViewModel
{
    public int Id { get; set; }
    
    [Display(Name = "TC Kimlik No")]
    public string TcKimlikNo { get; set; } = string.Empty;
    
    [Display(Name = "Ad")]
    public string FirstName { get; set; } = string.Empty;
    
    [Display(Name = "Soyad")]
    public string LastName { get; set; } = string.Empty;
    
    [Display(Name = "Ad Soyad")]
    public string FullName => $"{FirstName} {LastName}";
    
    [Display(Name = "Baba Adı")]
    public string? FatherName { get; set; }
    
    [Display(Name = "Anne Adı")]
    public string? MotherName { get; set; }
    
    [Display(Name = "Doğum Yeri")]
    public string? BirthPlace { get; set; }
    
    [Display(Name = "Doğum Tarihi")]
    [DataType(DataType.Date)]
    public DateTime? BirthDate { get; set; }
    
    [Display(Name = "E-posta")]
    public string? Email { get; set; }
    
    [Display(Name = "Telefon")]
    public string? Phone { get; set; }
    
    [Display(Name = "Adres")]
    public string? Address { get; set; }
    
    [Display(Name = "Personel No")]
    public string? EmployeeNumber { get; set; }
    
    [Display(Name = "İşe Giriş Tarihi")]
    [DataType(DataType.Date)]
    public DateTime? HireDate { get; set; }
    
    [Display(Name = "Maaş")]
    [DataType(DataType.Currency)]
    public decimal? Salary { get; set; }
    
    [Display(Name = "Pozisyon")]
    public string? Position { get; set; }
    
    [Display(Name = "Departman")]
    public string? DepartmentName { get; set; }
    
    [Display(Name = "Medeni Hal")]
    public string? MaritalStatus { get; set; }
    
    [Display(Name = "Kan Grubu")]
    public string? BloodType { get; set; }
    
    [Display(Name = "Din")]
    public string? Religion { get; set; }
    
    [Display(Name = "Askerlik Durumu")]
    public string? MilitaryStatus { get; set; }
    
    [Display(Name = "Ehliyet Sınıfı")]
    public string? DriverLicenseClass { get; set; }
    
    [Display(Name = "SSK No")]
    public string? SskNumber { get; set; }
    
    [Display(Name = "SSK Başlangıç Tarihi")]
    [DataType(DataType.Date)]
    public DateTime? SskStartDate { get; set; }
    
    [Display(Name = "Oluşturulma Tarihi")]
    [DataType(DataType.DateTime)]
    public DateTime CreatedAt { get; set; }
    
    [Display(Name = "Güncellenme Tarihi")]
    [DataType(DataType.DateTime)]
    public DateTime? UpdatedAt { get; set; }
    
    [Display(Name = "Aktif")]
    public bool IsActive { get; set; }
}
