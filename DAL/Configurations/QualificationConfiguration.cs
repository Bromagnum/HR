using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations;

public class QualificationConfiguration : IEntityTypeConfiguration<Qualification>
{
    public void Configure(EntityTypeBuilder<Qualification> builder)
    {
        builder.ToTable("Qualifications");

        builder.HasKey(q => q.Id);

        builder.Property(q => q.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(q => q.Category)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(q => q.IssuingAuthority)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(q => q.CredentialNumber)
            .HasMaxLength(100);

        builder.Property(q => q.IssueDate)
            .IsRequired();

        builder.Property(q => q.ExpirationDate);

        builder.Property(q => q.HasExpiration)
            .HasDefaultValue(false);

        builder.Property(q => q.Level)
            .HasMaxLength(100);

        builder.Property(q => q.Score);

        builder.Property(q => q.Description)
            .HasMaxLength(1000);

        builder.Property(q => q.AttachmentPath)
            .HasMaxLength(500);

        builder.Property(q => q.Location)
            .HasMaxLength(200);

        // Ignore computed properties
        builder.Ignore(q => q.IsExpired);
        builder.Ignore(q => q.IsExpiringSoon);

        // Foreign Key
        builder.HasOne(q => q.Person)
            .WithMany(p => p.Qualifications)
            .HasForeignKey(q => q.PersonId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(q => q.PersonId);
        builder.HasIndex(q => q.Category);
        builder.HasIndex(q => q.ExpirationDate);
        builder.HasIndex(q => new { q.PersonId, q.Category });
    }
}
