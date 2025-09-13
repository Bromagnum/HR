namespace DAL.Entities;

public class Organization : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Code { get; set; } = string.Empty;
    public int? ParentOrganizationId { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Manager { get; set; }
    public int? ManagerPersonId { get; set; }
    
    // Navigation Properties
    public Organization? ParentOrganization { get; set; }
    public ICollection<Organization> SubOrganizations { get; set; } = new List<Organization>();
    public Person? ManagerPerson { get; set; }
    public ICollection<Material> Materials { get; set; } = new List<Material>();
}
