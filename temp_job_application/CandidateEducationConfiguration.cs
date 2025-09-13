using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations;

public class CandidateEducationConfiguration : IEntityTypeConfiguration<CandidateEducation>
{
    public void Configure(EntityTypeBuilder<CandidateEducation> builder)
    {
        builder.ToTable("CandidateEducations");
        
        builder.HasKey(ce => ce.Id);
        
        // Indexes
        builder.HasIndex(ce => ce.CandidateId)
               .HasDatabaseName("IX_CandidateEducations_CandidateId");
        
        // Property configurations
        builder.Property(ce => ce.SchoolName)
               .IsRequired()
               .HasMaxLength(100);
               
        builder.Property(ce => ce.Degree)
               .IsRequired()
               .HasMaxLength(100);
               
        builder.Property(ce => ce.FieldOfStudy)
               .IsRequired()
               .HasMaxLength(100);
               
        builder.Property(ce => ce.StartDate)
               .IsRequired();
        
        // Decimal properties
        builder.Property(ce => ce.GPA)
               .HasPrecision(3, 2);
        
        // Relationships
        builder.HasOne(ce => ce.Candidate)
               .WithMany(c => c.Educations)
               .HasForeignKey(ce => ce.CandidateId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
