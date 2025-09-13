namespace BLL.DTOs;

public class MaterialListDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public int StockQuantity { get; set; }
    public int MinStockLevel { get; set; }
    public int MaxStockLevel { get; set; }
    public decimal TotalValue { get; set; }
    public string StockStatus { get; set; } = string.Empty;
    public bool IsLowStock { get; set; }
    public string? OrganizationName { get; set; }
    public string? Supplier { get; set; }
    public DateTime? LastPurchaseDate { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public bool IsActive { get; set; }
}

public class MaterialDetailDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Category { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public int StockQuantity { get; set; }
    public int MinStockLevel { get; set; }
    public int MaxStockLevel { get; set; }
    public decimal TotalValue { get; set; }
    public string StockStatus { get; set; } = string.Empty;
    public bool IsLowStock { get; set; }
    public bool IsOverStock { get; set; }
    public string? Supplier { get; set; }
    public string? Location { get; set; }
    public int? OrganizationId { get; set; }
    public string? OrganizationName { get; set; }
    public bool IsConsumable { get; set; }
    public DateTime? LastPurchaseDate { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class MaterialCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Category { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public int StockQuantity { get; set; }
    public int MinStockLevel { get; set; }
    public int MaxStockLevel { get; set; }
    public string? Supplier { get; set; }
    public string? Location { get; set; }
    public int? OrganizationId { get; set; }
    public bool IsConsumable { get; set; } = true;
    public DateTime? LastPurchaseDate { get; set; }
    public DateTime? ExpirationDate { get; set; }
}

public class MaterialUpdateDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Category { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public int StockQuantity { get; set; }
    public int MinStockLevel { get; set; }
    public int MaxStockLevel { get; set; }
    public string? Supplier { get; set; }
    public string? Location { get; set; }
    public int? OrganizationId { get; set; }
    public bool IsConsumable { get; set; }
    public DateTime? LastPurchaseDate { get; set; }
    public DateTime? ExpirationDate { get; set; }
}

public class MaterialFilterDto
{
    public string? Name { get; set; }
    public string? Code { get; set; }
    public string? Category { get; set; }
    public int? OrganizationId { get; set; }
    public bool? IsLowStock { get; set; }
    public bool? IsActive { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class MaterialStockSummaryDto
{
    public int TotalMaterials { get; set; }
    public int LowStockCount { get; set; }
    public int OverStockCount { get; set; }
    public decimal TotalStockValue { get; set; }
    public List<MaterialCategorySummaryDto> CategorySummary { get; set; } = new();
}

public class MaterialCategorySummaryDto
{
    public string Category { get; set; } = string.Empty;
    public int Count { get; set; }
    public decimal TotalValue { get; set; }
    public int LowStockCount { get; set; }
}
