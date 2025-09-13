using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations;

public class CandidateConfiguration : IEntityTypeConfiguration<Candidate>
{
    public void Configure(EntityTypeBuilder<Candidate> builder)
    {
        builder.ToTable("Candidates");
        
        builder.HasKey(c => c.Id);
        
        // Unique constraints
        builder.HasIndex(c => c.TcKimlikNo)
               .IsUnique()
               .HasDatabaseName("IX_Candidates_TcKimlikNo");
               
        builder.HasIndex(c => c.Email)
               .IsUnique()
               .HasDatabaseName("IX_Candidates_Email");
        
        // Property configurations
        builder.Property(c => c.TcKimlikNo)
               .IsRequired()
               .HasMaxLength(11)
               .IsFixedLength();
               
        builder.Property(c => c.FirstName)
               .IsRequired()
               .HasMaxLength(50);
               
        builder.Property(c => c.LastName)
               .IsRequired()
               .HasMaxLength(50);
               
        builder.Property(c => c.Email)
               .IsRequired()
               .HasMaxLength(100);
               
        builder.Property(c => c.Phone)
               .IsRequired()
               .HasMaxLength(20);
               
        builder.Property(c => c.Status)
               .IsRequired()
               .HasMaxLength(20)
               .HasDefaultValue("Active");
               
        builder.Property(c => c.Country)
               .HasMaxLength(100)
               .HasDefaultValue("Türkiye");
        
        // Decimal properties
        builder.Property(c => c.ExpectedSalary)
               .HasPrecision(18, 2);
               
        builder.Property(c => c.CurrentSalary)
               .HasPrecision(18, 2);
        
        // Relationships
        builder.HasMany(c => c.JobApplications)
               .WithOne(ja => ja.Candidate)
               .HasForeignKey(ja => ja.CandidateId)
               .OnDelete(DeleteBehavior.Cascade);
               
        builder.HasMany(c => c.Educations)
               .WithOne(ce => ce.Candidate)
               .HasForeignKey(ce => ce.CandidateId)
               .OnDelete(DeleteBehavior.Cascade);
               
        builder.HasMany(c => c.Experiences)
               .WithOne(ce => ce.Candidate)
               .HasForeignKey(ce => ce.CandidateId)
               .OnDelete(DeleteBehavior.Cascade);
               
        builder.HasMany(c => c.Skills)
               .WithOne(cs => cs.Candidate)
               .HasForeignKey(cs => cs.CandidateId)
               .OnDelete(DeleteBehavior.Cascade);
               
        builder.HasMany(c => c.InterviewNotes)
               .WithOne(i => i.Candidate)
               .HasForeignKey(i => i.CandidateId)
               .OnDelete(DeleteBehavior.SetNull);
        
        // Computed columns (if supported by your SQL Server version)
        builder.Property(c => c.FullName)
               .HasComputedColumnSql("CONCAT([FirstName], ' ', [LastName])")
               .ValueGeneratedOnAddOrUpdate();
    }
}
