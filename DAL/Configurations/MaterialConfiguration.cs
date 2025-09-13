using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations;

public class MaterialConfiguration : IEntityTypeConfiguration<Material>
{
    public void Configure(EntityTypeBuilder<Material> builder)
    {
        builder.ToTable("Materials");
        
        builder.HasKey(m => m.Id);
        
        builder.Property(m => m.Name)
            .IsRequired()
            .HasMaxLength(100);
            
        builder.Property(m => m.Code)
            .IsRequired()
            .HasMaxLength(20);
            
        builder.Property(m => m.Description)
            .HasMaxLength(500);
            
        builder.Property(m => m.Category)
            .IsRequired()
            .HasMaxLength(50);
            
        builder.Property(m => m.Unit)
            .IsRequired()
            .HasMaxLength(20);
            
        builder.Property(m => m.UnitPrice)
            .HasColumnType("decimal(18,2)");
            
        builder.Property(m => m.Supplier)
            .HasMaxLength(100);
            
        builder.Property(m => m.Location)
            .HasMaxLength(100);
        
        // Organization relationship
        builder.HasOne(m => m.Organization)
            .WithMany(o => o.Materials)
            .HasForeignKey(m => m.OrganizationId)
            .OnDelete(DeleteBehavior.SetNull);
            
        // Indexes
        builder.HasIndex(m => m.Code).IsUnique();
        builder.HasIndex(m => m.Name);
        builder.HasIndex(m => m.Category);
    }
}
