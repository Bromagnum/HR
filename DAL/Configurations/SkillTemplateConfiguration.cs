using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations;

public class SkillTemplateConfiguration : IEntityTypeConfiguration<SkillTemplate>
{
    public void Configure(EntityTypeBuilder<SkillTemplate> builder)
    {
        // Primary Key
        builder.HasKey(x => x.Id);

        // Properties
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Category)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Description)
            .HasMaxLength(1000);

        builder.Property(x => x.LevelDescriptions)
            .HasMaxLength(500);

        builder.Property(x => x.VerificationMethod)
            .HasMaxLength(200);

        builder.Property(x => x.Keywords)
            .HasMaxLength(1000);

        builder.Property(x => x.RelatedSkills)
            .HasMaxLength(1000);

        // Relationships
        builder.HasMany(x => x.PersonSkills)
            .WithOne(x => x.SkillTemplate)
            .HasForeignKey(x => x.SkillTemplateId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.JobRequiredSkills)
            .WithOne(x => x.SkillTemplate)
            .HasForeignKey(x => x.SkillTemplateId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(x => x.Name).IsUnique();
        builder.HasIndex(x => x.Category);
        builder.HasIndex(x => x.Type);

        // Table name
        builder.ToTable("SkillTemplates");
    }
}

public class PersonSkillConfiguration : IEntityTypeConfiguration<PersonSkill>
{
    public void Configure(EntityTypeBuilder<PersonSkill> builder)
    {
        // Primary Key
        builder.HasKey(x => x.Id);

        // Properties
        builder.Property(x => x.CertificationName)
            .HasMaxLength(200);

        builder.Property(x => x.CertificationAuthority)
            .HasMaxLength(200);

        builder.Property(x => x.Description)
            .HasMaxLength(1000);

        builder.Property(x => x.ProjectExamples)
            .HasMaxLength(1000);

        builder.Property(x => x.EndorsementNotes)
            .HasMaxLength(500);

        // Computed properties (not mapped to database)
        builder.Ignore(x => x.TotalExperience);
        builder.Ignore(x => x.IsCertificationExpired);
        builder.Ignore(x => x.IsCertificationExpiringSoon);

        // Relationships
        builder.HasOne(x => x.Person)
            .WithMany()
            .HasForeignKey(x => x.PersonId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.SkillTemplate)
            .WithMany(x => x.PersonSkills)
            .HasForeignKey(x => x.SkillTemplateId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.EndorsedBy)
            .WithMany()
            .HasForeignKey(x => x.EndorsedById)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.AssessedBy)
            .WithMany()
            .HasForeignKey(x => x.AssessedById)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(x => x.PersonId);
        builder.HasIndex(x => x.SkillTemplateId);
        builder.HasIndex(x => new { x.PersonId, x.SkillTemplateId }).IsUnique();
        builder.HasIndex(x => x.Level);
        builder.HasIndex(x => x.IsCertified);

        // Table name
        builder.ToTable("PersonSkills");
    }
}

public class JobRequiredSkillConfiguration : IEntityTypeConfiguration<JobRequiredSkill>
{
    public void Configure(EntityTypeBuilder<JobRequiredSkill> builder)
    {
        // Primary Key
        builder.HasKey(x => x.Id);

        // Properties
        builder.Property(x => x.SpecificRequirements)
            .HasMaxLength(1000);

        builder.Property(x => x.AssessmentCriteria)
            .HasMaxLength(500);

        // Relationships
        builder.HasOne(x => x.JobDefinition)
            .WithMany()
            .HasForeignKey(x => x.JobDefinitionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.SkillTemplate)
            .WithMany(x => x.JobRequiredSkills)
            .HasForeignKey(x => x.SkillTemplateId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(x => x.JobDefinitionId);
        builder.HasIndex(x => x.SkillTemplateId);
        builder.HasIndex(x => new { x.JobDefinitionId, x.SkillTemplateId }).IsUnique();
        builder.HasIndex(x => x.Importance);

        // Table name
        builder.ToTable("JobRequiredSkills");
    }
}

public class SkillAssessmentConfiguration : IEntityTypeConfiguration<SkillAssessment>
{
    public void Configure(EntityTypeBuilder<SkillAssessment> builder)
    {
        // Primary Key
        builder.HasKey(x => x.Id);

        // Properties
        builder.Property(x => x.Feedback)
            .HasMaxLength(1000);

        builder.Property(x => x.ImprovementAreas)
            .HasMaxLength(1000);

        builder.Property(x => x.Recommendations)
            .HasMaxLength(1000);

        builder.Property(x => x.AssessmentMethod)
            .HasMaxLength(200);

        // Relationships
        builder.HasOne(x => x.PersonSkill)
            .WithMany()
            .HasForeignKey(x => x.PersonSkillId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Assessor)
            .WithMany()
            .HasForeignKey(x => x.AssessorId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(x => x.PersonSkillId);
        builder.HasIndex(x => x.AssessorId);
        builder.HasIndex(x => x.AssessmentDate);
        builder.HasIndex(x => x.Type);

        // Table name
        builder.ToTable("SkillAssessments");
    }
}
