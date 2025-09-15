using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations;

public class ApplicationDocumentConfiguration : IEntityTypeConfiguration<ApplicationDocument>
{
    public void Configure(EntityTypeBuilder<ApplicationDocument> builder)
    {
        builder.ToTable("ApplicationDocuments");
        
        // Primary Key
        builder.HasKey(ad => ad.Id);
        
        // Properties
        builder.Property(ad => ad.FileName)
            .IsRequired()
            .HasMaxLength(255);
            
        builder.Property(ad => ad.FilePath)
            .IsRequired()
            .HasMaxLength(500);
            
        builder.Property(ad => ad.DocumentType)
            .IsRequired();
            
        builder.Property(ad => ad.FileSizeBytes)
            .IsRequired();
            
        builder.Property(ad => ad.MimeType)
            .IsRequired()
            .HasMaxLength(50);
            
        // Relationships
        builder.HasOne(ad => ad.JobApplication)
            .WithMany(ja => ja.Documents)
            .HasForeignKey(ad => ad.JobApplicationId)
            .OnDelete(DeleteBehavior.Cascade);
            
        // Computed properties are not mapped
        builder.Ignore(ad => ad.DocumentTypeText);
        builder.Ignore(ad => ad.FileSizeText);
        builder.Ignore(ad => ad.FileExtension);
        builder.Ignore(ad => ad.IsImageFile);
        builder.Ignore(ad => ad.IsPdfFile);
        builder.Ignore(ad => ad.IsDocumentFile);
        
        // Indexes
        builder.HasIndex(ad => ad.JobApplicationId)
            .HasDatabaseName("IX_ApplicationDocuments_JobApplicationId");
            
        builder.HasIndex(ad => ad.DocumentType)
            .HasDatabaseName("IX_ApplicationDocuments_DocumentType");
            
        builder.HasIndex(ad => ad.FileName)
            .HasDatabaseName("IX_ApplicationDocuments_FileName");
    }
}
