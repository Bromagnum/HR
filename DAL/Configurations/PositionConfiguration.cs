using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations
{
    public class PositionConfiguration : IEntityTypeConfiguration<Position>
    {
        public void Configure(EntityTypeBuilder<Position> builder)
        {
            // Table name
            builder.ToTable("Positions");

            // Primary key
            builder.HasKey(p => p.Id);

            // Properties
            builder.Property(p => p.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(p => p.Description)
                   .HasMaxLength(500);

            builder.Property(p => p.MinSalary)
                   .HasColumnType("decimal(18,2)");

            builder.Property(p => p.MaxSalary)
                   .HasColumnType("decimal(18,2)");

            builder.Property(p => p.RequiredExperience)
                   .HasDefaultValue(0);

            builder.Property(p => p.Requirements)
                   .HasMaxLength(1000);

            builder.Property(p => p.Responsibilities)
                   .HasMaxLength(1000);

            builder.Property(p => p.EmploymentType)
                   .HasMaxLength(50);

            builder.Property(p => p.Level)
                   .HasMaxLength(50);

            builder.Property(p => p.IsAvailable)
                   .HasDefaultValue(true);

            // Relationships
            builder.HasOne(p => p.Department)
                   .WithMany()
                   .HasForeignKey(p => p.DepartmentId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(p => p.Persons)
                   .WithOne(person => person.Position)
                   .HasForeignKey(person => person.PositionId)
                   .OnDelete(DeleteBehavior.SetNull);

            // Indexes
            builder.HasIndex(p => p.Name);
            builder.HasIndex(p => p.DepartmentId);
            builder.HasIndex(p => p.IsAvailable);
        }
    }
}
