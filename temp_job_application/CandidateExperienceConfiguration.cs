using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations;

public class CandidateExperienceConfiguration : IEntityTypeConfiguration<CandidateExperience>
{
    public void Configure(EntityTypeBuilder<CandidateExperience> builder)
    {
        builder.ToTable("CandidateExperiences");
        
        builder.HasKey(ce => ce.Id);
        
        // Indexes
        builder.HasIndex(ce => ce.CandidateId)
               .HasDatabaseName("IX_CandidateExperiences_CandidateId");
               
        builder.HasIndex(ce => ce.IsCurrentJob)
               .HasDatabaseName("IX_CandidateExperiences_IsCurrentJob");
        
        // Property configurations
        builder.Property(ce => ce.CompanyName)
               .IsRequired()
               .HasMaxLength(100);
               
        builder.Property(ce => ce.JobTitle)
               .IsRequired()
               .HasMaxLength(100);
               
        builder.Property(ce => ce.StartDate)
               .IsRequired();
        
        // Decimal properties
        builder.Property(ce => ce.Salary)
               .HasPrecision(18, 2);
        
        // Relationships
        builder.HasOne(ce => ce.Candidate)
               .WithMany(c => c.Experiences)
               .HasForeignKey(ce => ce.CandidateId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
