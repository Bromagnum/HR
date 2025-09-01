using System.ComponentModel.DataAnnotations;

namespace MVC.Models;

public class DepartmentFilterViewModel
{
    [Display(Name = "Arama")]
    public string? SearchTerm { get; set; }
    
    [Display(Name = "Durum")]
    public bool? IsActive { get; set; }
    
    [Display(Name = "Üst Departman")]
    public bool? HasParent { get; set; }
    
    [Display(Name = "Belirli Üst Departman")]
    public int? ParentDepartmentId { get; set; }
    
    [Display(Name = "Sıralama")]
    public string? SortBy { get; set; } = "Name";
    
    [Display(Name = "Azalan Sıralama")]
    public bool SortDescending { get; set; } = false;
    
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}

public class DepartmentSearchResultViewModel
{
    public IEnumerable<DepartmentListViewModel> Departments { get; set; } = new List<DepartmentListViewModel>();
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public int CurrentPage { get; set; }
    public bool HasPreviousPage => CurrentPage > 1;
    public bool HasNextPage => CurrentPage < TotalPages;
    public DepartmentFilterViewModel Filter { get; set; } = new();
    
    // Sayfalama helper'ları
    public int StartItem => TotalCount == 0 ? 0 : (CurrentPage - 1) * Filter.PageSize + 1;
    public int EndItem => Math.Min(CurrentPage * Filter.PageSize, TotalCount);
}
