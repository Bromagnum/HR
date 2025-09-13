using System.ComponentModel.DataAnnotations;

namespace MVC.Models;

public class OrganizationListViewModel
{
    public int Id { get; set; }
    
    [Display(Name = "Kod")]
    public string Code { get; set; } = string.Empty;
    
    [Display(Name = "Ad")]
    public string Name { get; set; } = string.Empty;
    
    [Display(Name = "Açıklama")]
    public string? Description { get; set; }
    
    [Display(Name = "Üst Organizasyon")]
    public string? ParentOrganizationName { get; set; }
    
    [Display(Name = "Yönetici")]
    public string? Manager { get; set; }
    
    [Display(Name = "Yönetici (Personel)")]
    public string? ManagerPersonName { get; set; }
    
    [Display(Name = "Tip")]
    public string Type { get; set; } = string.Empty;
    
    [Display(Name = "Telefon")]
    public string? Phone { get; set; }
    
    [Display(Name = "E-posta")]
    public string? Email { get; set; }
    
    [Display(Name = "Alt Organizasyonlar")]
    public int SubOrganizationsCount { get; set; }
    
    [Display(Name = "Malzeme Sayısı")]
    public int MaterialsCount { get; set; }
    
    [Display(Name = "Aktif")]
    public bool IsActive { get; set; }
    
    [Display(Name = "Oluşturma Tarihi")]
    public DateTime CreatedAt { get; set; }
}

public class OrganizationDetailViewModel
{
    public int Id { get; set; }
    
    [Display(Name = "Kod")]
    public string Code { get; set; } = string.Empty;
    
    [Display(Name = "Ad")]
    public string Name { get; set; } = string.Empty;
    
    [Display(Name = "Açıklama")]
    public string? Description { get; set; }
    
    [Display(Name = "Üst Organizasyon")]
    public string? ParentOrganizationName { get; set; }
    
    [Display(Name = "Adres")]
    public string? Address { get; set; }
    
    [Display(Name = "Telefon")]
    public string? Phone { get; set; }
    
    [Display(Name = "E-posta")]
    public string? Email { get; set; }
    
    [Display(Name = "Yönetici")]
    public string? Manager { get; set; }
    
    [Display(Name = "Yönetici (Personel)")]
    public string? ManagerPersonName { get; set; }
    
    [Display(Name = "Tip")]
    public string Type { get; set; } = string.Empty;
    
    [Display(Name = "Seviye")]
    public int Level { get; set; }
    
    [Display(Name = "Üst Organizasyon ID")]
    public int? ParentOrganizationId { get; set; }
    
    [Display(Name = "Alt Organizasyonlar")]
    public List<OrganizationListViewModel> SubOrganizations { get; set; } = new();
    
    [Display(Name = "Malzemeler")]
    public List<MaterialListViewModel> Materials { get; set; } = new();
    
    [Display(Name = "Aktif")]
    public bool IsActive { get; set; }
    
    [Display(Name = "Oluşturma Tarihi")]
    public DateTime CreatedAt { get; set; }
    
    [Display(Name = "Güncelleme Tarihi")]
    public DateTime? UpdatedAt { get; set; }
}

public class OrganizationCreateViewModel
{
    [Required(ErrorMessage = "Ad alanı zorunludur")]
    [StringLength(100, ErrorMessage = "Ad en fazla 100 karakter olabilir")]
    [Display(Name = "Ad")]
    public string Name { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Kod alanı zorunludur")]
    [StringLength(20, ErrorMessage = "Kod en fazla 20 karakter olabilir")]
    [Display(Name = "Kod")]
    public string Code { get; set; } = string.Empty;
    
    [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir")]
    [Display(Name = "Açıklama")]
    public string? Description { get; set; }
    
    [Display(Name = "Üst Organizasyon")]
    public int? ParentOrganizationId { get; set; }
    
    [StringLength(500, ErrorMessage = "Adres en fazla 500 karakter olabilir")]
    [Display(Name = "Adres")]
    public string? Address { get; set; }
    
    [StringLength(20, ErrorMessage = "Telefon en fazla 20 karakter olabilir")]
    [Display(Name = "Telefon")]
    public string? Phone { get; set; }
    
    [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz")]
    [StringLength(100, ErrorMessage = "E-posta en fazla 100 karakter olabilir")]
    [Display(Name = "E-posta")]
    public string? Email { get; set; }
    
    [StringLength(100, ErrorMessage = "Yönetici en fazla 100 karakter olabilir")]
    [Display(Name = "Yönetici")]
    public string? Manager { get; set; }
    
    [Display(Name = "Yönetici (Personel)")]
    public int? ManagerPersonId { get; set; }
    
    [Required(ErrorMessage = "Tip alanı zorunludur")]
    [Display(Name = "Tip")]
    public string Type { get; set; } = string.Empty;
    
    [Display(Name = "Seviye")]
    public int Level { get; set; }
    
    [Display(Name = "Aktif")]
    public bool IsActive { get; set; } = true;
}

public class OrganizationEditViewModel
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Ad alanı zorunludur")]
    [StringLength(100, ErrorMessage = "Ad en fazla 100 karakter olabilir")]
    [Display(Name = "Ad")]
    public string Name { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Kod alanı zorunludur")]
    [StringLength(20, ErrorMessage = "Kod en fazla 20 karakter olabilir")]
    [Display(Name = "Kod")]
    public string Code { get; set; } = string.Empty;
    
    [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir")]
    [Display(Name = "Açıklama")]
    public string? Description { get; set; }
    
    [Display(Name = "Üst Organizasyon")]
    public int? ParentOrganizationId { get; set; }
    
    [StringLength(500, ErrorMessage = "Adres en fazla 500 karakter olabilir")]
    [Display(Name = "Adres")]
    public string? Address { get; set; }
    
    [StringLength(20, ErrorMessage = "Telefon en fazla 20 karakter olabilir")]
    [Display(Name = "Telefon")]
    public string? Phone { get; set; }
    
    [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz")]
    [StringLength(100, ErrorMessage = "E-posta en fazla 100 karakter olabilir")]
    [Display(Name = "E-posta")]
    public string? Email { get; set; }
    
    [StringLength(100, ErrorMessage = "Yönetici en fazla 100 karakter olabilir")]
    [Display(Name = "Yönetici")]
    public string? Manager { get; set; }
    
    [Display(Name = "Yönetici (Personel)")]
    public int? ManagerPersonId { get; set; }
    
    [Required(ErrorMessage = "Tip alanı zorunludur")]
    [Display(Name = "Tip")]
    public string Type { get; set; } = string.Empty;
    
    [Display(Name = "Seviye")]
    public int Level { get; set; }
    
    [Display(Name = "Aktif")]
    public bool IsActive { get; set; }
    
    // Mevcut durumu gösteren property'ler
    [Display(Name = "Mevcut Üst Organizasyon")]
    public string? CurrentParentName { get; set; }
    
    [Display(Name = "Mevcut Yönetici")]
    public string? CurrentManagerName { get; set; }
    
    // Uyarı property'leri
    public bool HasSubOrganizations { get; set; }
    public bool HasMaterials { get; set; }
    
    // Sistem bilgileri
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class OrganizationFilterViewModel
{
    [Display(Name = "Ad")]
    public string? Name { get; set; }
    
    [Display(Name = "Kod")]
    public string? Code { get; set; }
    
    [Display(Name = "Üst Organizasyon")]
    public int? ParentOrganizationId { get; set; }
    
    [Display(Name = "Yönetici")]
    public string? Manager { get; set; }
    
    [Display(Name = "Aktif")]
    public bool? IsActive { get; set; }
    
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class OrganizationTreeViewModel
{
    public int Id { get; set; }
    
    [Display(Name = "Ad")]
    public string Name { get; set; } = string.Empty;
    
    [Display(Name = "Kod")]
    public string Code { get; set; } = string.Empty;
    
    [Display(Name = "Tip")]
    public string Type { get; set; } = string.Empty;
    
    [Display(Name = "Açıklama")]
    public string? Description { get; set; }
    
    [Display(Name = "Seviye")]
    public int Level { get; set; }
    
    [Display(Name = "Üst Organizasyon ID")]
    public int? ParentOrganizationId { get; set; }
    
    [Display(Name = "Yönetici")]
    public string? Manager { get; set; }
    
    [Display(Name = "Yönetici (Personel)")]
    public string? ManagerPersonName { get; set; }
    
    [Display(Name = "Aktif")]
    public bool IsActive { get; set; }
    
    [Display(Name = "Malzeme Sayısı")]
    public int MaterialsCount { get; set; }
    
    [Display(Name = "Alt Organizasyonlar")]
    public List<OrganizationTreeViewModel> SubOrganizations { get; set; } = new();
    
    public List<OrganizationTreeViewModel> Children { get; set; } = new();
}
