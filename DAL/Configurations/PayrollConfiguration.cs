using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations;

/// <summary>
/// Payroll Entity konfigürasyonu
/// Entity Framework için tablo yapısını tanımlar
/// </summary>
public class PayrollConfiguration : IEntityTypeConfiguration<Payroll>
{
    public void Configure(EntityTypeBuilder<Payroll> builder)
    {
        // Tablo adı ve check constraints
        builder.ToTable("Payrolls", t => 
        {
            t.HasCheckConstraint("CK_Payrolls_Year", "[Year] >= 2020 AND [Year] <= 2030");
            t.HasCheckConstraint("CK_Payrolls_Month", "[Month] >= 1 AND [Month] <= 12");
            t.HasCheckConstraint("CK_Payrolls_BasicSalary", "[BasicSalary] >= 0");
            t.HasCheckConstraint("CK_Payrolls_Allowances", "[Allowances] >= 0");
            t.HasCheckConstraint("CK_Payrolls_Bonuses", "[Bonuses] >= 0");
            t.HasCheckConstraint("CK_Payrolls_Deductions", "[Deductions] >= 0");
        });
        
        // Primary Key
        builder.HasKey(p => p.Id);
        
        // Properties (Özellikler)
        builder.Property(p => p.PersonId)
            .IsRequired()
            .HasComment("Bordronun ait olduğu personel ID");
            
        builder.Property(p => p.Year)
            .IsRequired()
            .HasComment("Bordro yılı");
            
        builder.Property(p => p.Month)
            .IsRequired()
            .HasComment("Bordro ayı (1-12)");
            
        builder.Property(p => p.BasicSalary)
            .IsRequired()
            .HasColumnType("decimal(18,2)")
            .HasComment("Temel maaş (brüt)");
            
        builder.Property(p => p.Allowances)
            .HasColumnType("decimal(18,2)")
            .HasDefaultValue(0)
            .HasComment("Ek ödemeler (prim, yemek yardımı vs.)");
            
        builder.Property(p => p.Bonuses)
            .HasColumnType("decimal(18,2)")
            .HasDefaultValue(0)
            .HasComment("İkramiyeler ve primler");
            
        builder.Property(p => p.Deductions)
            .HasColumnType("decimal(18,2)")
            .HasDefaultValue(0)
            .HasComment("Toplam kesintiler (vergi, SGK vs.)");
            
        builder.Property(p => p.NetSalary)
            .HasColumnType("decimal(18,2)")
            .HasComment("Net maaş");
            
        builder.Property(p => p.PaymentDate)
            .HasComment("Maaş ödeme tarihi");
            
        builder.Property(p => p.Description)
            .HasMaxLength(500)
            .HasComment("Bordro açıklaması");
            
        builder.Property(p => p.PreparedById)
            .HasComment("Bordroyu hazırlayan kişi ID");
            
        builder.Property(p => p.PreparedDate)
            .IsRequired()
            .HasDefaultValueSql("GETDATE()")
            .HasComment("Bordro hazırlanma tarihi");
        
        // Relationships (İlişkiler)
        // Payroll -> Person (Many to One)
        builder.HasOne(p => p.Person)
            .WithMany() // Person entity'sinde Payrolls koleksiyonu yok (basit tutuyoruz)
            .HasForeignKey(p => p.PersonId)
            .OnDelete(DeleteBehavior.Restrict) // Person silinirse bordro silinmez
            .HasConstraintName("FK_Payrolls_Persons_PersonId");
            
        // Payroll -> PreparedBy (Many to One - Optional)
        builder.HasOne(p => p.PreparedBy)
            .WithMany() // Person entity'sinde PreparedPayrolls koleksiyonu yok
            .HasForeignKey(p => p.PreparedById)
            .OnDelete(DeleteBehavior.SetNull) // PreparedBy silinirse NULL olur
            .HasConstraintName("FK_Payrolls_Persons_PreparedById");
        
        // Indexes (İndeksler) - Performans için
        builder.HasIndex(p => p.PersonId)
            .HasDatabaseName("IX_Payrolls_PersonId");
            
        builder.HasIndex(p => new { p.Year, p.Month })
            .HasDatabaseName("IX_Payrolls_Year_Month");
            
        builder.HasIndex(p => new { p.PersonId, p.Year, p.Month })
            .IsUnique() // Aynı kişi için aynı ay-yılda sadece 1 bordro
            .HasDatabaseName("IX_Payrolls_PersonId_Year_Month_Unique");
    }
}
