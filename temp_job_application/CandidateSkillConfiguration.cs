using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations;

public class CandidateSkillConfiguration : IEntityTypeConfiguration<CandidateSkill>
{
    public void Configure(EntityTypeBuilder<CandidateSkill> builder)
    {
        builder.ToTable("CandidateSkills");
        
        builder.HasKey(cs => cs.Id);
        
        // Indexes
        builder.HasIndex(cs => cs.CandidateId)
               .HasDatabaseName("IX_CandidateSkills_CandidateId");
               
        builder.HasIndex(cs => cs.SkillName)
               .HasDatabaseName("IX_CandidateSkills_SkillName");
               
        builder.HasIndex(cs => cs.Category)
               .HasDatabaseName("IX_CandidateSkills_Category");
        
        // Property configurations
        builder.Property(cs => cs.SkillName)
               .IsRequired()
               .HasMaxLength(100);
               
        builder.Property(cs => cs.Level)
               .IsRequired()
               .HasConversion<int>();
               
        builder.Property(cs => cs.Category)
               .IsRequired()
               .HasConversion<int>();
        
        // Relationships
        builder.HasOne(cs => cs.Candidate)
               .WithMany(c => c.Skills)
               .HasForeignKey(cs => cs.CandidateId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
