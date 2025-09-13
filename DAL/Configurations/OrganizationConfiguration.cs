using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations;

public class OrganizationConfiguration : IEntityTypeConfiguration<Organization>
{
    public void Configure(EntityTypeBuilder<Organization> builder)
    {
        builder.ToTable("Organizations");
        
        builder.HasKey(o => o.Id);
        
        builder.Property(o => o.Name)
            .IsRequired()
            .HasMaxLength(100);
            
        builder.Property(o => o.Code)
            .IsRequired()
            .HasMaxLength(20);
            
        builder.Property(o => o.Description)
            .HasMaxLength(500);
            
        builder.Property(o => o.Address)
            .HasMaxLength(500);
            
        builder.Property(o => o.Phone)
            .HasMaxLength(20);
            
        builder.Property(o => o.Email)
            .HasMaxLength(100);
            
        builder.Property(o => o.Manager)
            .HasMaxLength(100);
        
        // Self-referencing relationship
        builder.HasOne(o => o.ParentOrganization)
            .WithMany(o => o.SubOrganizations)
            .HasForeignKey(o => o.ParentOrganizationId)
            .OnDelete(DeleteBehavior.NoAction);
            
        // Manager relationship
        builder.HasOne(o => o.ManagerPerson)
            .WithMany()
            .HasForeignKey(o => o.ManagerPersonId)
            .OnDelete(DeleteBehavior.SetNull);
            
        // Index
        builder.HasIndex(o => o.Code).IsUnique();
        builder.HasIndex(o => o.Name);
    }
}
