using System.ComponentModel.DataAnnotations;

namespace MVC.Models;

public class DepartmentListViewModel
{
    public int Id { get; set; }
    
    [Display(Name = "Departman Adı")]
    public string Name { get; set; } = string.Empty;
    
    [Display(Name = "Açıklama")]
    public string? Description { get; set; }
    
    [Display(Name = "Üst Departman")]
    public string? ParentDepartmentName { get; set; }
    
    [Display(Name = "Personel Sayısı")]
    public int EmployeeCount { get; set; }
    
    [Display(Name = "Aktif")]
    public bool IsActive { get; set; }
    
    [Display(Name = "Seviye")]
    public int Level { get; set; }
    
    public bool HasChildren { get; set; }
}

public class DepartmentDetailViewModel
{
    public int Id { get; set; }
    
    [Display(Name = "Departman Adı")]
    public string Name { get; set; } = string.Empty;
    
    [Display(Name = "Açıklama")]
    public string? Description { get; set; }
    
    [Display(Name = "Üst Departman ID")]
    public int? ParentDepartmentId { get; set; }
    
    [Display(Name = "Üst Departman")]
    public string? ParentDepartmentName { get; set; }
    
    [Display(Name = "Alt Departmanlar")]
    public List<DepartmentListViewModel> SubDepartments { get; set; } = new();
    
    [Display(Name = "Personeller")]
    public List<PersonListViewModel> Employees { get; set; } = new();
    
    [Display(Name = "Oluşturulma Tarihi")]
    [DataType(DataType.DateTime)]
    public DateTime CreatedAt { get; set; }
    
    [Display(Name = "Güncellenme Tarihi")]
    [DataType(DataType.DateTime)]
    public DateTime? UpdatedAt { get; set; }
    
    [Display(Name = "Aktif")]
    public bool IsActive { get; set; }
}

public class DepartmentCreateViewModel
{
    [Required(ErrorMessage = "Departman adı zorunludur")]
    [StringLength(100, ErrorMessage = "Departman adı en fazla 100 karakter olabilir")]
    [Display(Name = "Departman Adı")]
    public string Name { get; set; } = string.Empty;
    
    [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir")]
    [Display(Name = "Açıklama")]
    public string? Description { get; set; }
    
    [Display(Name = "Üst Departman")]
    public int? ParentDepartmentId { get; set; }
}

public class DepartmentEditViewModel : DepartmentCreateViewModel
{
    public int Id { get; set; }
    
    [Display(Name = "Aktif")]
    public bool IsActive { get; set; } = true;
}

public class DepartmentTreeViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int EmployeeCount { get; set; }
    public bool IsActive { get; set; }
    public int Level { get; set; }
    public List<DepartmentTreeViewModel> Children { get; set; } = new();
}
