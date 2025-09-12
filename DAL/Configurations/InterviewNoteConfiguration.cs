using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations;

public class InterviewNoteConfiguration : IEntityTypeConfiguration<InterviewNote>
{
    public void Configure(EntityTypeBuilder<InterviewNote> builder)
    {
        builder.ToTable("InterviewNotes");
        
        builder.HasKey(i => i.Id);
        
        // Indexes
        builder.HasIndex(i => i.JobApplicationId)
               .HasDatabaseName("IX_InterviewNotes_JobApplicationId");
               
        builder.HasIndex(i => i.CandidateId)
               .HasDatabaseName("IX_InterviewNotes_CandidateId");
               
        builder.HasIndex(i => i.InterviewerId)
               .HasDatabaseName("IX_InterviewNotes_InterviewerId");
               
        builder.HasIndex(i => i.InterviewDate)
               .HasDatabaseName("IX_InterviewNotes_InterviewDate");
        
        // Property configurations
        builder.Property(i => i.InterviewDate)
               .IsRequired();
               
        builder.Property(i => i.InterviewType)
               .IsRequired()
               .HasConversion<int>();
        
        // Relationships
        builder.HasOne(i => i.JobApplication)
               .WithMany(ja => ja.InterviewNotes)
               .HasForeignKey(i => i.JobApplicationId)
               .OnDelete(DeleteBehavior.Cascade);
               
        builder.HasOne(i => i.Candidate)
               .WithMany(c => c.InterviewNotes)
               .HasForeignKey(i => i.CandidateId)
               .OnDelete(DeleteBehavior.SetNull);
               
        builder.HasOne(i => i.Interviewer)
               .WithMany()
               .HasForeignKey(i => i.InterviewerId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
