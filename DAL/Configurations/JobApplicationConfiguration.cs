using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations;

public class JobApplicationConfiguration : IEntityTypeConfiguration<JobApplication>
{
    public void Configure(EntityTypeBuilder<JobApplication> builder)
    {
        builder.ToTable("JobApplications", t =>
        {
            t.HasComment("İş başvuruları tablosu");
            t.HasCheckConstraint("CK_JobApplications_Age", "[Age] >= 18 AND [Age] <= 70");
            t.HasCheckConstraint("CK_JobApplications_ExperienceYears", "[ExperienceYears] >= 0 AND [ExperienceYears] <= 50");
            t.HasCheckConstraint("CK_JobApplications_ExpectedSalary", "[ExpectedSalary] >= 0");
            t.HasCheckConstraint("CK_JobApplications_Rating", "[Rating] >= 1 AND [Rating] <= 10");
            t.HasCheckConstraint("CK_JobApplications_GraduationYear", "[GraduationYear] >= 1950 AND [GraduationYear] <= 2030");
        });

        // Primary Key
        builder.HasKey(x => x.Id);

        // Properties
        builder.Property(x => x.FirstName)
            .IsRequired()
            .HasMaxLength(100)
            .HasComment("Başvuru sahibinin adı");

        builder.Property(x => x.LastName)
            .IsRequired()
            .HasMaxLength(100)
            .HasComment("Başvuru sahibinin soyadı");

        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(200)
            .HasComment("E-posta adresi");

        builder.Property(x => x.Phone)
            .IsRequired()
            .HasMaxLength(20)
            .HasComment("Telefon numarası");

        builder.Property(x => x.NationalId)
            .HasMaxLength(11)
            .HasComment("TC Kimlik numarası");

        builder.Property(x => x.Address)
            .HasMaxLength(500)
            .HasComment("Adres bilgisi");

        builder.Property(x => x.Status)
            .IsRequired()
            .HasComment("Başvuru durumu");

        builder.Property(x => x.ApplicationDate)
            .IsRequired()
            .HasComment("Başvuru tarihi");

        builder.Property(x => x.CoverLetter)
            .HasMaxLength(2000)
            .HasComment("Ön yazı");

        builder.Property(x => x.CurrentCompany)
            .HasMaxLength(200)
            .HasComment("Mevcut şirket");

        builder.Property(x => x.CurrentPosition)
            .HasMaxLength(100)
            .HasComment("Mevcut pozisyon");

        builder.Property(x => x.EducationLevel)
            .HasMaxLength(100)
            .HasComment("Eğitim seviyesi");

        builder.Property(x => x.University)
            .HasMaxLength(200)
            .HasComment("Üniversite");

        builder.Property(x => x.Department)
            .HasMaxLength(100)
            .HasComment("Bölüm");

        builder.Property(x => x.Skills)
            .HasMaxLength(1000)
            .HasComment("Yetenekler");

        builder.Property(x => x.Languages)
            .HasMaxLength(500)
            .HasComment("Diller");

        builder.Property(x => x.ReviewNotes)
            .HasMaxLength(2000)
            .HasComment("İnceleme notları");

        builder.Property(x => x.InterviewNotes)
            .HasMaxLength(1000)
            .HasComment("Mülakat notları");

        builder.Property(x => x.ExpectedSalary)
            .HasColumnType("decimal(18,2)")
            .HasComment("Beklenen maaş");

        // Relationships
        builder.HasOne(x => x.Position)
            .WithMany()
            .HasForeignKey(x => x.PositionId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_JobApplications_Positions");

        builder.HasOne(x => x.ReviewedBy)
            .WithMany()
            .HasForeignKey(x => x.ReviewedById)
            .OnDelete(DeleteBehavior.SetNull)
            .HasConstraintName("FK_JobApplications_ReviewedBy");

        builder.HasMany(x => x.Documents)
            .WithOne(x => x.JobApplication)
            .HasForeignKey(x => x.JobApplicationId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_ApplicationDocuments_JobApplications");

        // Indexes
        builder.HasIndex(x => x.Email)
            .HasDatabaseName("IX_JobApplications_Email");

        builder.HasIndex(x => x.PositionId)
            .HasDatabaseName("IX_JobApplications_PositionId");

        builder.HasIndex(x => x.Status)
            .HasDatabaseName("IX_JobApplications_Status");

        builder.HasIndex(x => x.ApplicationDate)
            .HasDatabaseName("IX_JobApplications_ApplicationDate");

        builder.HasIndex(x => new { x.FirstName, x.LastName })
            .HasDatabaseName("IX_JobApplications_FullName");

        // Ignore computed properties
        builder.Ignore(x => x.FullName);
        builder.Ignore(x => x.StatusText);
        builder.Ignore(x => x.StatusClass);
        builder.Ignore(x => x.HasCV);
        builder.Ignore(x => x.HasCoverLetter);
        builder.Ignore(x => x.DocumentCount);
        builder.Ignore(x => x.ApplicationPeriod);
    }
}
