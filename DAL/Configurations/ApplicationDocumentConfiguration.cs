using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations;

public class ApplicationDocumentConfiguration : IEntityTypeConfiguration<ApplicationDocument>
{
    public void Configure(EntityTypeBuilder<ApplicationDocument> builder)
    {
        builder.ToTable("ApplicationDocuments");
        
        builder.HasKey(ad => ad.Id);
        
        // Indexes
        builder.HasIndex(ad => ad.JobApplicationId)
               .HasDatabaseName("IX_ApplicationDocuments_JobApplicationId");
               
        builder.HasIndex(ad => ad.DocumentType)
               .HasDatabaseName("IX_ApplicationDocuments_DocumentType");
               
        builder.HasIndex(ad => ad.UploadDate)
               .HasDatabaseName("IX_ApplicationDocuments_UploadDate");
        
        // Property configurations
        builder.Property(ad => ad.FileName)
               .IsRequired()
               .HasMaxLength(200);
               
        builder.Property(ad => ad.FilePath)
               .IsRequired()
               .HasMaxLength(500);
               
        builder.Property(ad => ad.DocumentType)
               .IsRequired()
               .HasConversion<int>();
               
        builder.Property(ad => ad.FileSizeBytes)
               .IsRequired();
               
        builder.Property(ad => ad.MimeType)
               .IsRequired()
               .HasMaxLength(50);
               
        builder.Property(ad => ad.UploadDate)
               .IsRequired()
               .HasDefaultValueSql("GETDATE()");
        
        // Relationships
        builder.HasOne(ad => ad.JobApplication)
               .WithMany(ja => ja.Documents)
               .HasForeignKey(ad => ad.JobApplicationId)
               .OnDelete(DeleteBehavior.Cascade);
               
        builder.HasOne(ad => ad.UploadedBy)
               .WithMany()
               .HasForeignKey(ad => ad.UploadedById)
               .OnDelete(DeleteBehavior.SetNull);
               
        builder.HasOne(ad => ad.VerifiedBy)
               .WithMany()
               .HasForeignKey(ad => ad.VerifiedById)
               .OnDelete(DeleteBehavior.SetNull);
               
        builder.HasOne(ad => ad.LastDownloadedBy)
               .WithMany()
               .HasForeignKey(ad => ad.LastDownloadedById)
               .OnDelete(DeleteBehavior.SetNull);
    }
}
