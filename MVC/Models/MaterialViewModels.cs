using System.ComponentModel.DataAnnotations;

namespace MVC.Models;

public class MaterialListViewModel
{
    public int Id { get; set; }
    
    [Display(Name = "Kod")]
    public string Code { get; set; } = string.Empty;
    
    [Display(Name = "Ad")]
    public string Name { get; set; } = string.Empty;
    
    [Display(Name = "Kategori")]
    public string Category { get; set; } = string.Empty;
    
    [Display(Name = "Birim")]
    public string Unit { get; set; } = string.Empty;
    
    [Display(Name = "Birim Fiyat")]
    [DisplayFormat(DataFormatString = "{0:C2}")]
    public decimal UnitPrice { get; set; }
    
    [Display(Name = "Stok Miktarı")]
    public int StockQuantity { get; set; }
    
    [Display(Name = "Min. Stok")]
    public int MinStockLevel { get; set; }
    
    [Display(Name = "Max. Stok")]
    public int MaxStockLevel { get; set; }
    
    [Display(Name = "Toplam Değer")]
    [DisplayFormat(DataFormatString = "{0:C2}")]
    public decimal TotalValue { get; set; }
    
    [Display(Name = "Stok Durumu")]
    public string StockStatus { get; set; } = string.Empty;
    
    [Display(Name = "Düşük Stok")]
    public bool IsLowStock { get; set; }
    
    [Display(Name = "Organizasyon")]
    public string? OrganizationName { get; set; }
    
    [Display(Name = "Tedarikçi")]
    public string? Supplier { get; set; }
    
    [Display(Name = "Son Alım Tarihi")]
    [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
    public DateTime? LastPurchaseDate { get; set; }
    
    [Display(Name = "Son Kullanma Tarihi")]
    [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
    public DateTime? ExpirationDate { get; set; }
    
    [Display(Name = "Aktif")]
    public bool IsActive { get; set; }
}

public class MaterialDetailViewModel
{
    public int Id { get; set; }
    
    [Display(Name = "Kod")]
    public string Code { get; set; } = string.Empty;
    
    [Display(Name = "Ad")]
    public string Name { get; set; } = string.Empty;
    
    [Display(Name = "Açıklama")]
    public string? Description { get; set; }
    
    [Display(Name = "Kategori")]
    public string Category { get; set; } = string.Empty;
    
    [Display(Name = "Birim")]
    public string Unit { get; set; } = string.Empty;
    
    [Display(Name = "Birim Fiyat")]
    [DisplayFormat(DataFormatString = "{0:C2}")]
    public decimal UnitPrice { get; set; }
    
    [Display(Name = "Stok Miktarı")]
    public int StockQuantity { get; set; }
    
    [Display(Name = "Min. Stok Seviyesi")]
    public int MinStockLevel { get; set; }
    
    [Display(Name = "Minimum Stok")]
    public int MinStockQuantity { get; set; }
    
    [Display(Name = "Max. Stok Seviyesi")]
    public int MaxStockLevel { get; set; }
    
    [Display(Name = "Maksimum Stok")]
    public int MaxStockQuantity { get; set; }
    
    [Display(Name = "Toplam Değer")]
    [DisplayFormat(DataFormatString = "{0:C2}")]
    public decimal TotalValue { get; set; }
    
    [Display(Name = "Stok Durumu")]
    public string StockStatus { get; set; } = string.Empty;
    
    [Display(Name = "Düşük Stok")]
    public bool IsLowStock { get; set; }
    
    [Display(Name = "Fazla Stok")]
    public bool IsOverStock { get; set; }
    
    [Display(Name = "Tedarikçi")]
    public string? Supplier { get; set; }
    
    [Display(Name = "Lokasyon")]
    public string? Location { get; set; }
    
    [Display(Name = "Organizasyon ID")]
    public int OrganizationId { get; set; }
    
    [Display(Name = "Organizasyon")]
    public string? OrganizationName { get; set; }
    
    [Display(Name = "Tüketim Malzemesi")]
    public bool IsConsumable { get; set; }
    
    [Display(Name = "Son Alım Tarihi")]
    [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
    public DateTime? LastPurchaseDate { get; set; }
    
    [Display(Name = "Son Kullanma Tarihi")]
    [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
    public DateTime? ExpirationDate { get; set; }
    
    [Display(Name = "Aktif")]
    public bool IsActive { get; set; }
    
    [Display(Name = "Oluşturma Tarihi")]
    [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}")]
    public DateTime CreatedAt { get; set; }
    
    [Display(Name = "Güncelleme Tarihi")]
    [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}")]
    public DateTime? UpdatedAt { get; set; }
}

public class MaterialCreateViewModel
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
    
    [Required(ErrorMessage = "Kategori alanı zorunludur")]
    [StringLength(50, ErrorMessage = "Kategori en fazla 50 karakter olabilir")]
    [Display(Name = "Kategori")]
    public string Category { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Birim alanı zorunludur")]
    [StringLength(20, ErrorMessage = "Birim en fazla 20 karakter olabilir")]
    [Display(Name = "Birim")]
    public string Unit { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Birim fiyat alanı zorunludur")]
    [Range(0.01, 999999.99, ErrorMessage = "Birim fiyat 0.01 ile 999999.99 arasında olmalıdır")]
    [Display(Name = "Birim Fiyat")]
    public decimal UnitPrice { get; set; }
    
    [Required(ErrorMessage = "Stok miktarı alanı zorunludur")]
    [Range(0, int.MaxValue, ErrorMessage = "Stok miktarı 0 veya pozitif olmalıdır")]
    [Display(Name = "Stok Miktarı")]
    public int StockQuantity { get; set; }
    
    [Required(ErrorMessage = "Min. stok seviyesi alanı zorunludur")]
    [Range(0, int.MaxValue, ErrorMessage = "Min. stok seviyesi 0 veya pozitif olmalıdır")]
    [Display(Name = "Min. Stok Seviyesi")]
    public int MinStockLevel { get; set; }
    
    [Required(ErrorMessage = "Max. stok seviyesi alanı zorunludur")]
    [Range(0, int.MaxValue, ErrorMessage = "Max. stok seviyesi 0 veya pozitif olmalıdır")]
    [Display(Name = "Max. Stok Seviyesi")]
    public int MaxStockLevel { get; set; }
    
    [StringLength(100, ErrorMessage = "Tedarikçi en fazla 100 karakter olabilir")]
    [Display(Name = "Tedarikçi")]
    public string? Supplier { get; set; }
    
    [StringLength(100, ErrorMessage = "Lokasyon en fazla 100 karakter olabilir")]
    [Display(Name = "Lokasyon")]
    public string? Location { get; set; }
    
    [Display(Name = "Organizasyon")]
    public int? OrganizationId { get; set; }
    
    [Display(Name = "Tüketim Malzemesi")]
    public bool IsConsumable { get; set; } = true;
    
    [Display(Name = "Son Alım Tarihi")]
    public DateTime? LastPurchaseDate { get; set; }
    
    [Display(Name = "Son Kullanma Tarihi")]
    public DateTime? ExpirationDate { get; set; }
    
    [Required(ErrorMessage = "Minimum stok alanı zorunludur")]
    [Range(0, int.MaxValue, ErrorMessage = "Minimum stok 0 veya pozitif olmalıdır")]
    [Display(Name = "Minimum Stok")]
    public int MinStockQuantity { get; set; }
    
    [Required(ErrorMessage = "Maksimum stok alanı zorunludur")]
    [Range(1, int.MaxValue, ErrorMessage = "Maksimum stok 1 veya pozitif olmalıdır")]
    [Display(Name = "Maksimum Stok")]
    public int MaxStockQuantity { get; set; }
    
    [Display(Name = "Aktif")]
    public bool IsActive { get; set; } = true;
}

public class MaterialEditViewModel
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
    
    [Required(ErrorMessage = "Kategori alanı zorunludur")]
    [StringLength(50, ErrorMessage = "Kategori en fazla 50 karakter olabilir")]
    [Display(Name = "Kategori")]
    public string Category { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Birim alanı zorunludur")]
    [StringLength(20, ErrorMessage = "Birim en fazla 20 karakter olabilir")]
    [Display(Name = "Birim")]
    public string Unit { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Birim fiyat alanı zorunludur")]
    [Range(0.01, 999999.99, ErrorMessage = "Birim fiyat 0.01 ile 999999.99 arasında olmalıdır")]
    [Display(Name = "Birim Fiyat")]
    public decimal UnitPrice { get; set; }
    
    [Required(ErrorMessage = "Stok miktarı alanı zorunludur")]
    [Range(0, int.MaxValue, ErrorMessage = "Stok miktarı 0 veya pozitif olmalıdır")]
    [Display(Name = "Stok Miktarı")]
    public int StockQuantity { get; set; }
    
    [Required(ErrorMessage = "Min. stok seviyesi alanı zorunludur")]
    [Range(0, int.MaxValue, ErrorMessage = "Min. stok seviyesi 0 veya pozitif olmalıdır")]
    [Display(Name = "Min. Stok Seviyesi")]
    public int MinStockLevel { get; set; }
    
    [Required(ErrorMessage = "Max. stok seviyesi alanı zorunludur")]
    [Range(0, int.MaxValue, ErrorMessage = "Max. stok seviyesi 0 veya pozitif olmalıdır")]
    [Display(Name = "Max. Stok Seviyesi")]
    public int MaxStockLevel { get; set; }
    
    [StringLength(100, ErrorMessage = "Tedarikçi en fazla 100 karakter olabilir")]
    [Display(Name = "Tedarikçi")]
    public string? Supplier { get; set; }
    
    [StringLength(100, ErrorMessage = "Lokasyon en fazla 100 karakter olabilir")]
    [Display(Name = "Lokasyon")]
    public string? Location { get; set; }
    
    [Display(Name = "Organizasyon")]
    public int? OrganizationId { get; set; }
    
    [Display(Name = "Tüketim Malzemesi")]
    public bool IsConsumable { get; set; }
    
    [Display(Name = "Son Alım Tarihi")]
    public DateTime? LastPurchaseDate { get; set; }
    
    [Display(Name = "Son Kullanma Tarihi")]
    public DateTime? ExpirationDate { get; set; }
}

public class MaterialFilterViewModel
{
    [Display(Name = "Ad")]
    public string? Name { get; set; }
    
    [Display(Name = "Kod")]
    public string? Code { get; set; }
    
    [Display(Name = "Kategori")]
    public string? Category { get; set; }
    
    [Display(Name = "Organizasyon")]
    public int? OrganizationId { get; set; }
    
    [Display(Name = "Sadece Düşük Stok")]
    public bool? IsLowStock { get; set; }
    
    [Display(Name = "Aktif")]
    public bool? IsActive { get; set; }
    
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class MaterialStockSummaryViewModel
{
    [Display(Name = "Toplam Malzeme")]
    public int TotalMaterials { get; set; }
    
    [Display(Name = "Düşük Stok")]
    public int LowStockCount { get; set; }
    
    [Display(Name = "Fazla Stok")]
    public int OverStockCount { get; set; }
    
    [Display(Name = "Toplam Stok Değeri")]
    [DisplayFormat(DataFormatString = "{0:C2}")]
    public decimal TotalStockValue { get; set; }
    
    [Display(Name = "Kategoriler")]
    public List<MaterialCategorySummaryViewModel> CategorySummary { get; set; } = new();
}

public class MaterialCategorySummaryViewModel
{
    [Display(Name = "Kategori")]
    public string Category { get; set; } = string.Empty;
    
    [Display(Name = "Adet")]
    public int Count { get; set; }
    
    [Display(Name = "Toplam Değer")]
    [DisplayFormat(DataFormatString = "{0:C2}")]
    public decimal TotalValue { get; set; }
    
    [Display(Name = "Düşük Stok")]
    public int LowStockCount { get; set; }
}
