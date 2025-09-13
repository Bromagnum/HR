namespace BLL.DTOs;

public class PersonListDto
{
    public int Id { get; set; }
    public string TcKimlikNo { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName => $"{FirstName} {LastName}";
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? EmployeeNumber { get; set; }
    public string? Position { get; set; }
    public string? DepartmentName { get; set; }
    public int? DepartmentId { get; set; }
    public string? PositionName { get; set; }
    public DateTime? HireDate { get; set; }
    public decimal? Salary { get; set; }
    public bool IsActive { get; set; }
}

public class PersonDetailDto
{
    public int Id { get; set; }
    
    // Kimlik Bilgileri
    public string TcKimlikNo { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName => $"{FirstName} {LastName}";
    public string? FatherName { get; set; }
    public string? MotherName { get; set; }
    public string? BirthPlace { get; set; }
    public DateTime? BirthDate { get; set; }
    
    // İletişim Bilgileri
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    
    // İş Bilgileri
    public string? EmployeeNumber { get; set; }
    public DateTime? HireDate { get; set; }
    public decimal? Salary { get; set; }
    public string? Position { get; set; }
    public int? DepartmentId { get; set; }
    public string? DepartmentName { get; set; }
    
    // Kişisel Bilgiler
    public string? MaritalStatus { get; set; }
    public string? BloodType { get; set; }
    public string? Religion { get; set; }
    public string? MilitaryStatus { get; set; }
    public string? DriverLicenseClass { get; set; }
    
    // SSK Bilgileri
    public string? SskNumber { get; set; }
    public DateTime? SskStartDate { get; set; }
    
    // Audit
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; }
}

public class PersonCreateDto
{
    // Kimlik Bilgileri
    public string TcKimlikNo { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? FatherName { get; set; }
    public string? MotherName { get; set; }
    public string? BirthPlace { get; set; }
    public DateTime? BirthDate { get; set; }
    
    // İletişim Bilgileri
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    
    // İş Bilgileri
    public string? EmployeeNumber { get; set; }
    public DateTime? HireDate { get; set; }
    public decimal? Salary { get; set; }
    public string? Position { get; set; }
    public int? DepartmentId { get; set; }
    
    // Kişisel Bilgiler
    public string? MaritalStatus { get; set; }
    public string? BloodType { get; set; }
    public string? Religion { get; set; }
    public string? MilitaryStatus { get; set; }
    public string? DriverLicenseClass { get; set; }
    
    // SSK Bilgileri
    public string? SskNumber { get; set; }
    public DateTime? SskStartDate { get; set; }
}

public class PersonUpdateDto
{
    public int Id { get; set; }
    
    // Kimlik Bilgileri
    public string TcKimlikNo { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? FatherName { get; set; }
    public string? MotherName { get; set; }
    public string? BirthPlace { get; set; }
    public DateTime? BirthDate { get; set; }
    
    // İletişim Bilgileri
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    
    // İş Bilgileri
    public string? EmployeeNumber { get; set; }
    public DateTime? HireDate { get; set; }
    public decimal? Salary { get; set; }
    public string? Position { get; set; }
    public int? DepartmentId { get; set; }
    
    // Kişisel Bilgiler
    public string? MaritalStatus { get; set; }
    public string? BloodType { get; set; }
    public string? Religion { get; set; }
    public string? MilitaryStatus { get; set; }
    public string? DriverLicenseClass { get; set; }
    
    // SSK Bilgileri
    public string? SskNumber { get; set; }
    public DateTime? SskStartDate { get; set; }
    
    public bool IsActive { get; set; } = true;
}
