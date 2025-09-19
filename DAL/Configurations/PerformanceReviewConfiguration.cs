using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations;

public class PerformanceReviewConfiguration : IEntityTypeConfiguration<PerformanceReview>
{
    public void Configure(EntityTypeBuilder<PerformanceReview> builder)
    {
        builder.ToTable("PerformanceReviews");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.OverallScore)
            .IsRequired()
            .HasComment("Genel değerlendirme skoru (1-5)");
            
        builder.Property(x => x.JobQualityScore)
            .IsRequired()
            .HasComment("İş kalitesi skoru (1-5)");
            
        builder.Property(x => x.ProductivityScore)
            .IsRequired()
            .HasComment("Üretkenlik skoru (1-5)");
            
        builder.Property(x => x.TeamworkScore)
            .IsRequired()
            .HasComment("Ekip çalışması skoru (1-5)");
            
        builder.Property(x => x.CommunicationScore)
            .IsRequired()
            .HasComment("İletişim skoru (1-5)");
            
        builder.Property(x => x.LeadershipScore)
            .IsRequired()
            .HasComment("Liderlik skoru (1-5)");
            
        builder.Property(x => x.InitiativeScore)
            .IsRequired()
            .HasComment("İnisiyatif skoru (1-5)");
            
        builder.Property(x => x.ProblemSolvingScore)
            .IsRequired()
            .HasComment("Problem çözme skoru (1-5)");
            
        builder.Property(x => x.AdaptabilityScore)
            .IsRequired()
            .HasComment("Uyum skoru (1-5)");
        
        builder.Property(x => x.Strengths)
            .HasMaxLength(2000)
            .HasComment("Güçlü yönler");
            
        builder.Property(x => x.AreasForImprovement)
            .HasMaxLength(2000)
            .HasComment("Gelişim alanları");
            
        builder.Property(x => x.Achievements)
            .HasMaxLength(2000)
            .HasComment("Başarılar");
            
        builder.Property(x => x.Goals)
            .HasMaxLength(2000)
            .HasComment("Hedefler");
            
        builder.Property(x => x.ReviewerComments)
            .HasMaxLength(2000)
            .HasComment("Değerlendiren yorumları");
            
        builder.Property(x => x.EmployeeComments)
            .HasMaxLength(2000)
            .HasComment("Çalışan yorumları");
            
        builder.Property(x => x.SelfAssessmentComments)
            .HasMaxLength(2000)
            .HasComment("Öz değerlendirme yorumları");
        
        builder.Property(x => x.Status)
            .IsRequired()
            .HasConversion<int>()
            .HasComment("Değerlendirme durumu");
        
        // Foreign Key Relationships
        builder.HasOne(x => x.Person)
            .WithMany()
            .HasForeignKey(x => x.PersonId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_PerformanceReviews_Person");
            
        builder.HasOne(x => x.ReviewPeriod)
            .WithMany(x => x.PerformanceReviews)
            .HasForeignKey(x => x.ReviewPeriodId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_PerformanceReviews_ReviewPeriod");
            
        builder.HasOne(x => x.Reviewer)
            .WithMany()
            .HasForeignKey(x => x.ReviewerId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_PerformanceReviews_Reviewer");
            
        builder.HasOne(x => x.ApprovedBy)
            .WithMany()
            .HasForeignKey(x => x.ApprovedById)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_PerformanceReviews_ApprovedBy");
        
        // Indexes
        builder.HasIndex(x => x.PersonId)
            .HasDatabaseName("IX_PerformanceReviews_PersonId");
            
        builder.HasIndex(x => x.ReviewPeriodId)
            .HasDatabaseName("IX_PerformanceReviews_ReviewPeriodId");
            
        builder.HasIndex(x => x.ReviewerId)
            .HasDatabaseName("IX_PerformanceReviews_ReviewerId");
            
        builder.HasIndex(x => x.Status)
            .HasDatabaseName("IX_PerformanceReviews_Status");
            
        builder.HasIndex(x => new { x.PersonId, x.ReviewPeriodId })
            .IsUnique()
            .HasDatabaseName("IX_PerformanceReviews_Person_Period");
    }
}

public class ReviewPeriodConfiguration : IEntityTypeConfiguration<ReviewPeriod>
{
    public void Configure(EntityTypeBuilder<ReviewPeriod> builder)
    {
        builder.ToTable("ReviewPeriods");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100)
            .HasComment("Değerlendirme dönemi adı");
            
        builder.Property(x => x.Description)
            .HasMaxLength(500)
            .HasComment("Değerlendirme dönemi açıklaması");
        
        builder.Property(x => x.StartDate)
            .IsRequired()
            .HasComment("Dönem başlangıç tarihi");
            
        builder.Property(x => x.EndDate)
            .IsRequired()
            .HasComment("Dönem bitiş tarihi");
            
        builder.Property(x => x.ReviewStartDate)
            .IsRequired()
            .HasComment("Değerlendirme başlangıç tarihi");
            
        builder.Property(x => x.ReviewEndDate)
            .IsRequired()
            .HasComment("Değerlendirme bitiş tarihi");
        
        builder.Property(x => x.Type)
            .IsRequired()
            .HasConversion<int>()
            .HasComment("Değerlendirme dönemi türü");
        
        // Indexes
        builder.HasIndex(x => x.Name)
            .IsUnique()
            .HasDatabaseName("IX_ReviewPeriods_Name");
            
        builder.HasIndex(x => x.IsActive)
            .HasDatabaseName("IX_ReviewPeriods_IsActive");
            
        builder.HasIndex(x => new { x.StartDate, x.EndDate })
            .HasDatabaseName("IX_ReviewPeriods_DateRange");
    }
}

public class PerformanceGoalConfiguration : IEntityTypeConfiguration<PerformanceGoal>
{
    public void Configure(EntityTypeBuilder<PerformanceGoal> builder)
    {
        builder.ToTable("PerformanceGoals");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(200)
            .HasComment("Hedef başlığı");
            
        builder.Property(x => x.Description)
            .HasMaxLength(1000)
            .HasComment("Hedef açıklaması");
            
        builder.Property(x => x.Notes)
            .HasMaxLength(1000)
            .HasComment("Hedef notları");
        
        builder.Property(x => x.Status)
            .IsRequired()
            .HasConversion<int>()
            .HasComment("Hedef durumu");
            
        builder.Property(x => x.Priority)
            .IsRequired()
            .HasConversion<int>()
            .HasComment("Hedef önceliği");
        
        builder.Property(x => x.ProgressPercentage)
            .IsRequired()
            .HasComment("İlerleme yüzdesi (0-100)");
        
        // Foreign Key Relationships
        builder.HasOne(x => x.PerformanceReview)
            .WithMany(x => x.Goals_Navigation)
            .HasForeignKey(x => x.PerformanceReviewId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_PerformanceGoals_PerformanceReview");
        
        // Indexes
        builder.HasIndex(x => x.PerformanceReviewId)
            .HasDatabaseName("IX_PerformanceGoals_PerformanceReviewId");
            
        builder.HasIndex(x => x.Status)
            .HasDatabaseName("IX_PerformanceGoals_Status");
            
        builder.HasIndex(x => x.TargetDate)
            .HasDatabaseName("IX_PerformanceGoals_TargetDate");
    }
}
