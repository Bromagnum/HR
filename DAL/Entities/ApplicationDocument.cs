using System.ComponentModel.DataAnnotations;

namespace DAL.Entities;

public enum DocumentType
{
    CV = 1,
    CoverLetter = 2,
    Portfolio = 3,
    Certificate = 4,
    Transcript = 5,
    Reference = 6,
    IdentityDocument = 7,
    Other = 8
}

public class ApplicationDocument : BaseEntity
{
    [Required]
    public int JobApplicationId { get; set; }
    public virtual JobApplication JobApplication { get; set; } = null!;
    
    [Required]
    [StringLength(200)]
    public string FileName { get; set; } = string.Empty;
    
    [Required]
    [StringLength(500)]
    public string FilePath { get; set; } = string.Empty;
    
    [Required]
    public DocumentType DocumentType { get; set; } = DocumentType.Other;
    
    [Required]
    [Range(1, long.MaxValue)]
    public long FileSizeBytes { get; set; }
    
    [Required]
    [StringLength(50)]
    public string MimeType { get; set; } = string.Empty;
    
    [StringLength(500)]
    public string? Description { get; set; }
    
    public DateTime UploadDate { get; set; } = DateTime.Now;
    
    public int? UploadedById { get; set; }
    public virtual ApplicationUser? UploadedBy { get; set; }
    
    public bool IsVerified { get; set; } = false;
    
    public int? VerifiedById { get; set; }
    public virtual ApplicationUser? VerifiedBy { get; set; }
    
    public DateTime? VerifiedAt { get; set; }
    
    [StringLength(1000)]
    public string? VerificationNotes { get; set; }
    
    public int DownloadCount { get; set; } = 0;
    
    public DateTime? LastDownloadedAt { get; set; }
    
    public int? LastDownloadedById { get; set; }
    public virtual ApplicationUser? LastDownloadedBy { get; set; }
    
    // Computed Properties
    public string DocumentTypeText => DocumentType switch
    {
        DocumentType.CV => "CV/Özgeçmiş",
        DocumentType.CoverLetter => "Ön Yazı",
        DocumentType.Portfolio => "Portföy",
        DocumentType.Certificate => "Sertifika",
        DocumentType.Transcript => "Transkript",
        DocumentType.Reference => "Referans Mektubu",
        DocumentType.IdentityDocument => "Kimlik Belgesi",
        DocumentType.Other => "Diğer",
        _ => "Bilinmiyor"
    };
    
    public string FileSizeText
    {
        get
        {
            if (FileSizeBytes < 1024)
                return $"{FileSizeBytes} B";
            else if (FileSizeBytes < 1024 * 1024)
                return $"{Math.Round((double)FileSizeBytes / 1024, 1)} KB";
            else
                return $"{Math.Round((double)FileSizeBytes / (1024 * 1024), 1)} MB";
        }
    }
    
    public string FileExtension => Path.GetExtension(FileName).ToLowerInvariant();
    
    public bool IsImageFile => new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" }.Contains(FileExtension);
    
    public bool IsPdfFile => FileExtension == ".pdf";
    
    public bool IsDocumentFile => new[] { ".doc", ".docx", ".pdf", ".txt", ".rtf" }.Contains(FileExtension);
}
