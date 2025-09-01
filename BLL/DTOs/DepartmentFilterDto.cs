namespace BLL.DTOs;

public class DepartmentFilterDto
{
    public string? SearchTerm { get; set; }
    public bool? IsActive { get; set; }
    public bool? HasParent { get; set; }
    public int? ParentDepartmentId { get; set; }
    public string? SortBy { get; set; } = "Name";
    public bool SortDescending { get; set; } = false;
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 50;
}

public class DepartmentSearchResultDto
{
    public IEnumerable<DepartmentListDto> Departments { get; set; } = new List<DepartmentListDto>();
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public int CurrentPage { get; set; }
    public bool HasPreviousPage => CurrentPage > 1;
    public bool HasNextPage => CurrentPage < TotalPages;
    public DepartmentFilterDto Filter { get; set; } = new();
}
