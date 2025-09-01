namespace DAL.Entities;

public class Department : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? ParentDepartmentId { get; set; }
    
    // Navigation Properties
    public Department? ParentDepartment { get; set; }
    public ICollection<Department> SubDepartments { get; set; } = new List<Department>();
    public ICollection<Person> Employees { get; set; } = new List<Person>();
}
