using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations;

public class EducationConfiguration : IEntityTypeConfiguration<Education>
{
    public void Configure(EntityTypeBuilder<Education> builder)
    {
        builder.ToTable("Educations");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.SchoolName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Degree)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.FieldOfStudy)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.StartDate)
            .IsRequired();

        builder.Property(e => e.EndDate);

        builder.Property(e => e.IsOngoing)
            .HasDefaultValue(false);

        builder.Property(e => e.GPA)
            .HasColumnType("decimal(3,2)");

        builder.Property(e => e.Description)
            .HasMaxLength(500);

        builder.Property(e => e.Location)
            .HasMaxLength(100);

        // Foreign Key
        builder.HasOne(e => e.Person)
            .WithMany(p => p.Educations)
            .HasForeignKey(e => e.PersonId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(e => e.PersonId);
        builder.HasIndex(e => new { e.PersonId, e.StartDate });
    }
}

