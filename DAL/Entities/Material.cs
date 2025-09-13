namespace DAL.Entities;

public class Material : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty; // Adet, Kg, Lt, vs.
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
    
    // Navigation Properties
    public Organization? Organization { get; set; }
    
    // Computed Properties
    public bool IsLowStock => StockQuantity <= MinStockLevel;
    public bool IsOverStock => StockQuantity >= MaxStockLevel;
    public decimal TotalValue => StockQuantity * UnitPrice;
    public string StockStatus => IsLowStock ? "Düşük Stok" : IsOverStock ? "Fazla Stok" : "Normal";
}
