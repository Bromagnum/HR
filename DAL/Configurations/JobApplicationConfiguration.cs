using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations;

public class JobApplicationConfiguration : IEntityTypeConfiguration<JobApplication>
{
    public void Configure(EntityTypeBuilder<JobApplication> builder)
    {
        builder.ToTable("JobApplications");
        
        builder.HasKey(ja => ja.Id);
        
        // Indexes
        builder.HasIndex(ja => ja.CandidateId)
               .HasDatabaseName("IX_JobApplications_CandidateId");
               
        builder.HasIndex(ja => ja.PositionId)
               .HasDatabaseName("IX_JobApplications_PositionId");
               
        builder.HasIndex(ja => ja.Status)
               .HasDatabaseName("IX_JobApplications_Status");
               
        builder.HasIndex(ja => ja.ApplicationDate)
               .HasDatabaseName("IX_JobApplications_ApplicationDate");
        
        // Property configurations
        builder.Property(ja => ja.Status)
               .IsRequired()
               .HasConversion<int>();
               
        builder.Property(ja => ja.ApplicationDate)
               .IsRequired()
               .HasDefaultValueSql("GETDATE()");
        
        // Decimal properties
        builder.Property(ja => ja.RequestedSalary)
               .HasPrecision(18, 2);
               
        builder.Property(ja => ja.OfferedSalary)
               .HasPrecision(18, 2);
        
        // Relationships
        builder.HasOne(ja => ja.Candidate)
               .WithMany(c => c.JobApplications)
               .HasForeignKey(ja => ja.CandidateId)
               .OnDelete(DeleteBehavior.Cascade);
               
        builder.HasOne(ja => ja.Position)
               .WithMany()
               .HasForeignKey(ja => ja.PositionId)
               .OnDelete(DeleteBehavior.Restrict);
               
        builder.HasOne(ja => ja.ReviewedBy)
               .WithMany()
               .HasForeignKey(ja => ja.ReviewedById)
               .OnDelete(DeleteBehavior.SetNull);
               
        builder.HasOne(ja => ja.Interviewer)
               .WithMany()
               .HasForeignKey(ja => ja.InterviewerId)
               .OnDelete(DeleteBehavior.SetNull);
               
        builder.HasOne(ja => ja.DecisionBy)
               .WithMany()
               .HasForeignKey(ja => ja.DecisionById)
               .OnDelete(DeleteBehavior.SetNull);
               
        builder.HasOne(ja => ja.LastViewedBy)
               .WithMany()
               .HasForeignKey(ja => ja.LastViewedById)
               .OnDelete(DeleteBehavior.SetNull);
        
        builder.HasMany(ja => ja.InterviewNotes)
               .WithOne(i => i.JobApplication)
               .HasForeignKey(i => i.JobApplicationId)
               .OnDelete(DeleteBehavior.Cascade);
               
        builder.HasMany(ja => ja.Documents)
               .WithOne(d => d.JobApplication)
               .HasForeignKey(d => d.JobApplicationId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
