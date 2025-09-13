namespace BLL.DTOs;

public class OrganizationListDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? ParentOrganizationId { get; set; }
    public string? ParentOrganizationName { get; set; }
    public string? Manager { get; set; }
    public string? ManagerPersonName { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public int SubOrganizationsCount { get; set; }
    public int MaterialsCount { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class OrganizationDetailDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? ParentOrganizationId { get; set; }
    public string? ParentOrganizationName { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Manager { get; set; }
    public int? ManagerPersonId { get; set; }
    public string? ManagerPersonName { get; set; }
    public List<OrganizationListDto> SubOrganizations { get; set; } = new();
    public List<MaterialListDto> Materials { get; set; } = new();
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class OrganizationCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? ParentOrganizationId { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Manager { get; set; }
    public int? ManagerPersonId { get; set; }
}

public class OrganizationUpdateDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? ParentOrganizationId { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Manager { get; set; }
    public int? ManagerPersonId { get; set; }
}

public class OrganizationFilterDto
{
    public string? Name { get; set; }
    public string? Code { get; set; }
    public int? ParentOrganizationId { get; set; }
    public string? Manager { get; set; }
    public bool? IsActive { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class OrganizationTreeDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? Manager { get; set; }
    public int MaterialsCount { get; set; }
    public List<OrganizationTreeDto> Children { get; set; } = new();
}
