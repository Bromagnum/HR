using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations;

public class JobDefinitionConfiguration : IEntityTypeConfiguration<JobDefinition>
{
    public void Configure(EntityTypeBuilder<JobDefinition> builder)
    {
        // Primary Key
        builder.HasKey(x => x.Id);

        // Properties
        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.DetailedDescription)
            .IsRequired()
            .HasMaxLength(5000);

        builder.Property(x => x.MainResponsibilities)
            .HasMaxLength(2000);

        builder.Property(x => x.SecondaryResponsibilities)
            .HasMaxLength(2000);

        builder.Property(x => x.RequiredSkills)
            .HasMaxLength(2000);

        builder.Property(x => x.PreferredSkills)
            .HasMaxLength(2000);

        builder.Property(x => x.RequiredCertifications)
            .HasMaxLength(1000);

        builder.Property(x => x.PreferredCertifications)
            .HasMaxLength(1000);

        builder.Property(x => x.TechnicalSkills)
            .HasMaxLength(1000);

        builder.Property(x => x.SoftSkills)
            .HasMaxLength(1000);

        builder.Property(x => x.Languages)
            .HasMaxLength(500);

        builder.Property(x => x.PhysicalRequirements)
            .HasMaxLength(1000);

        builder.Property(x => x.WorkingConditions)
            .HasMaxLength(1000);

        builder.Property(x => x.CareerPath)
            .HasMaxLength(1000);

        builder.Property(x => x.PerformanceMetrics)
            .HasMaxLength(1000);

        builder.Property(x => x.Version)
            .HasMaxLength(50)
            .IsRequired();

        // Relationships
        builder.HasOne(x => x.Position)
            .WithMany()
            .HasForeignKey(x => x.PositionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ApprovedBy)
            .WithMany()
            .HasForeignKey(x => x.ApprovedById)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.PreviousVersion)
            .WithMany()
            .HasForeignKey(x => x.PreviousVersionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.RequiredQualifications)
            .WithOne(x => x.JobDefinition)
            .HasForeignKey(x => x.JobDefinitionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.MatchingResults)
            .WithOne(x => x.JobDefinition)
            .HasForeignKey(x => x.JobDefinitionId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(x => x.PositionId);
        builder.HasIndex(x => x.IsApproved);
        builder.HasIndex(x => new { x.PositionId, x.Version });

        // Table name
        builder.ToTable("JobDefinitions");
    }
}

public class JobDefinitionQualificationConfiguration : IEntityTypeConfiguration<JobDefinitionQualification>
{
    public void Configure(EntityTypeBuilder<JobDefinitionQualification> builder)
    {
        // Primary Key
        builder.HasKey(x => x.Id);

        // Properties
        builder.Property(x => x.QualificationName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Category)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Description)
            .HasMaxLength(500);

        // Relationships
        builder.HasOne(x => x.JobDefinition)
            .WithMany(x => x.RequiredQualifications)
            .HasForeignKey(x => x.JobDefinitionId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(x => x.JobDefinitionId);
        builder.HasIndex(x => new { x.JobDefinitionId, x.Category });

        // Table name
        builder.ToTable("JobDefinitionQualifications");
    }
}

public class QualificationMatchingResultConfiguration : IEntityTypeConfiguration<QualificationMatchingResult>
{
    public void Configure(EntityTypeBuilder<QualificationMatchingResult> builder)
    {
        // Primary Key
        builder.HasKey(x => x.Id);

        // Properties
        builder.Property(x => x.OverallMatchPercentage)
            .HasPrecision(5, 2);

        builder.Property(x => x.RequiredSkillsMatch)
            .HasPrecision(5, 2);

        builder.Property(x => x.PreferredSkillsMatch)
            .HasPrecision(5, 2);

        builder.Property(x => x.ExperienceMatch)
            .HasPrecision(5, 2);

        builder.Property(x => x.EducationMatch)
            .HasPrecision(5, 2);

        builder.Property(x => x.CertificationMatch)
            .HasPrecision(5, 2);

        builder.Property(x => x.MatchingDetails)
            .HasMaxLength(2000);

        builder.Property(x => x.MissingRequirements)
            .HasMaxLength(1000);

        builder.Property(x => x.Recommendations)
            .HasMaxLength(1000);

        builder.Property(x => x.ReviewNotes)
            .HasMaxLength(1000);

        // Relationships
        builder.HasOne(x => x.JobDefinition)
            .WithMany(x => x.MatchingResults)
            .HasForeignKey(x => x.JobDefinitionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Person)
            .WithMany()
            .HasForeignKey(x => x.PersonId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ReviewedBy)
            .WithMany()
            .HasForeignKey(x => x.ReviewedById)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(x => x.JobDefinitionId);
        builder.HasIndex(x => x.PersonId);
        builder.HasIndex(x => new { x.JobDefinitionId, x.PersonId }).IsUnique();
        builder.HasIndex(x => x.OverallMatchPercentage);
        builder.HasIndex(x => x.Status);

        // Table name
        builder.ToTable("QualificationMatchingResults");
    }
}
