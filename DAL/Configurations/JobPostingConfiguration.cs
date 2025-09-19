using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations;

public class JobPostingConfiguration : IEntityTypeConfiguration<JobPosting>
{
    public void Configure(EntityTypeBuilder<JobPosting> builder)
    {
        builder.ToTable("JobPostings", t =>
        {
            t.HasComment("İş ilanları tablosu");
            t.HasCheckConstraint("CK_JobPostings_MinExperience", "[MinExperience] >= 0 AND [MinExperience] <= 50");
            t.HasCheckConstraint("CK_JobPostings_MaxExperience", "[MaxExperience] >= 0 AND [MaxExperience] <= 50");
            t.HasCheckConstraint("CK_JobPostings_MinSalary", "[MinSalary] >= 0");
            t.HasCheckConstraint("CK_JobPostings_MaxSalary", "[MaxSalary] >= 0");
            t.HasCheckConstraint("CK_JobPostings_OpenPositions", "[OpenPositions] >= 1 AND [OpenPositions] <= 100");
            t.HasCheckConstraint("CK_JobPostings_ViewCount", "[ViewCount] >= 0");
            t.HasCheckConstraint("CK_JobPostings_ApplicationCount", "[ApplicationCount] >= 0");
        });

        // Primary Key
        builder.HasKey(x => x.Id);

        // Properties
        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(200)
            .HasComment("İş ilanı başlığı");

        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(5000)
            .HasComment("İş tanımı");

        builder.Property(x => x.Status)
            .IsRequired()
            .HasComment("İlan durumu");

        builder.Property(x => x.EmploymentType)
            .IsRequired()
            .HasComment("Çalışma türü");

        builder.Property(x => x.Requirements)
            .HasMaxLength(1000)
            .HasComment("Gereksinimler");

        builder.Property(x => x.Responsibilities)
            .HasMaxLength(1000)
            .HasComment("Sorumluluklar");

        builder.Property(x => x.Benefits)
            .HasMaxLength(1000)
            .HasComment("Yan haklar");

        builder.Property(x => x.MinEducation)
            .HasMaxLength(100)
            .HasComment("Minimum eğitim seviyesi");

        builder.Property(x => x.Location)
            .HasMaxLength(200)
            .HasComment("Çalışma yeri");

        builder.Property(x => x.ContactInfo)
            .HasMaxLength(500)
            .HasComment("İletişim bilgileri");

        builder.Property(x => x.Slug)
            .HasMaxLength(200)
            .HasComment("SEO dostu URL");

        builder.Property(x => x.MetaDescription)
            .HasMaxLength(500)
            .HasComment("Meta açıklama");

        builder.Property(x => x.Tags)
            .HasMaxLength(200)
            .HasComment("Etiketler");

        builder.Property(x => x.PublishDate)
            .IsRequired()
            .HasComment("Yayınlanma tarihi");

        builder.Property(x => x.MinSalary)
            .HasColumnType("decimal(18,2)")
            .HasComment("Minimum maaş");

        builder.Property(x => x.MaxSalary)
            .HasColumnType("decimal(18,2)")
            .HasComment("Maksimum maaş");

        // Relationships
        builder.HasOne(x => x.Position)
            .WithMany()
            .HasForeignKey(x => x.PositionId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_JobPostings_Positions");

        builder.HasOne(x => x.Department)
            .WithMany()
            .HasForeignKey(x => x.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_JobPostings_Departments");

        builder.HasOne(x => x.CreatedBy)
            .WithMany()
            .HasForeignKey(x => x.CreatedById)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_JobPostings_CreatedBy");

        builder.HasOne(x => x.UpdatedBy)
            .WithMany()
            .HasForeignKey(x => x.UpdatedById)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_JobPostings_UpdatedBy");

        builder.HasMany(x => x.Applications)
            .WithOne()
            .HasForeignKey("JobPostingId")
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_JobApplications_JobPostings");

        // Indexes
        builder.HasIndex(x => x.Title)
            .HasDatabaseName("IX_JobPostings_Title");

        builder.HasIndex(x => x.Status)
            .HasDatabaseName("IX_JobPostings_Status");

        builder.HasIndex(x => x.PositionId)
            .HasDatabaseName("IX_JobPostings_PositionId");

        builder.HasIndex(x => x.DepartmentId)
            .HasDatabaseName("IX_JobPostings_DepartmentId");

        builder.HasIndex(x => x.PublishDate)
            .HasDatabaseName("IX_JobPostings_PublishDate");

        builder.HasIndex(x => x.ExpiryDate)
            .HasDatabaseName("IX_JobPostings_ExpiryDate");

        builder.HasIndex(x => x.Slug)
            .IsUnique()
            .HasDatabaseName("IX_JobPostings_Slug");

        // Ignore computed properties
        builder.Ignore(x => x.StatusText);
        builder.Ignore(x => x.StatusClass);
        builder.Ignore(x => x.EmploymentTypeText);
        builder.Ignore(x => x.IsExpired);
        builder.Ignore(x => x.IsApplicationDeadlinePassed);
        builder.Ignore(x => x.IsActive);
        builder.Ignore(x => x.SalaryRange);
        builder.Ignore(x => x.ExperienceRange);
        builder.Ignore(x => x.DaysUntilExpiry);
        builder.Ignore(x => x.DaysUntilApplicationDeadline);
    }
}
