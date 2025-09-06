namespace DAL.Entities;

public class Person : BaseEntity
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
    public int? PositionId { get; set; }
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
    
    // Navigation Properties
    public Department? Department { get; set; }
    public Position? Position { get; set; }
    public virtual ICollection<Education> Educations { get; set; } = new List<Education>();
    public virtual ICollection<Qualification> Qualifications { get; set; } = new List<Qualification>();
    public virtual ICollection<Leave> Leaves { get; set; } = new List<Leave>();
    public virtual ICollection<Leave> ApprovedLeaves { get; set; } = new List<Leave>();
    public virtual ICollection<Leave> HandoverLeaves { get; set; } = new List<Leave>();
    public virtual ICollection<LeaveBalance> LeaveBalances { get; set; } = new List<LeaveBalance>();
}
