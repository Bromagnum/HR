using System.ComponentModel.DataAnnotations;

namespace DAL.Entities;

/// <summary>
/// Bordro (Maaş Bordrosu) Entity
/// Basit bordro işlemleri için kullanılır
/// </summary>
public class Payroll : BaseEntity
{
    /// <summary>
    /// Hangi personelin bordrosu
    /// </summary>
    [Required]
    public int PersonId { get; set; }
    
    /// <summary>
    /// Bordro yılı (örn: 2024)
    /// </summary>
    [Required]
    [Range(2020, 2030)]
    public int Year { get; set; } = DateTime.Now.Year;
    
    /// <summary>
    /// Bordro ayı (1-12)
    /// </summary>
    [Required]
    [Range(1, 12)]
    public int Month { get; set; } = DateTime.Now.Month;
    
    /// <summary>
    /// Temel maaş (brüt)
    /// </summary>
    [Required]
    [Range(0, 999999)]
    [Display(Name = "Temel Maaş")]
    public decimal BasicSalary { get; set; }
    
    /// <summary>
    /// Ek ödemeler (prim, ikramiye vs.)
    /// </summary>
    [Range(0, 999999)]
    [Display(Name = "Ek Ödemeler")]
    public decimal Allowances { get; set; } = 0;
    
    /// <summary>
    /// İkramiyeler ve primler
    /// </summary>
    [Range(0, 999999)]
    [Display(Name = "İkramiyeler")]
    public decimal Bonuses { get; set; } = 0;
    
    /// <summary>
    /// Toplam kesintiler (vergi, SGK vs.)
    /// </summary>
    [Range(0, 999999)]
    [Display(Name = "Kesintiler")]
    public decimal Deductions { get; set; } = 0;
    
    /// <summary>
    /// Net maaş (hesaplanacak)
    /// </summary>
    [Display(Name = "Net Maaş")]
    public decimal NetSalary { get; set; }
    
    /// <summary>
    /// Bordro açıklaması
    /// </summary>
    [StringLength(500)]
    [Display(Name = "Açıklama")]
    public string? Description { get; set; }
    
    /// <summary>
    /// Bordroyu hazırlayan kişi
    /// </summary>
    public int? PreparedById { get; set; }
    
    /// <summary>
    /// Bordro hazırlanma tarihi
    /// </summary>
    public DateTime PreparedDate { get; set; } = DateTime.Now;
    
    /// <summary>
    /// Maaş ödeme tarihi
    /// </summary>
    [Display(Name = "Ödeme Tarihi")]
    public DateTime? PaymentDate { get; set; }
    
    // Navigation Properties
    /// <summary>
    /// Bordronun ait olduğu personel
    /// </summary>
    public virtual Person Person { get; set; } = null!;
    
    /// <summary>
    /// Bordroyu hazırlayan kişi (opsiyonel)
    /// </summary>
    public virtual Person? PreparedBy { get; set; }
    
    // Computed Properties (Hesaplanan özellikler)
    /// <summary>
    /// Brüt maaş = Temel Maaş + Ek Ödemeler + İkramiyeler
    /// </summary>
    public decimal GrossSalary => BasicSalary + Allowances + Bonuses;
    
    /// <summary>
    /// Ay adı (Ocak, Şubat vs.)
    /// </summary>
    public string MonthName => Month switch
    {
        1 => "Ocak", 2 => "Şubat", 3 => "Mart", 4 => "Nisan",
        5 => "Mayıs", 6 => "Haziran", 7 => "Temmuz", 8 => "Ağustos",
        9 => "Eylül", 10 => "Ekim", 11 => "Kasım", 12 => "Aralık",
        _ => "Bilinmiyor"
    };
    
    /// <summary>
    /// Bordro dönemi (Ocak 2024 formatında)
    /// </summary>
    public string PayrollPeriod => $"{MonthName} {Year}";
}
