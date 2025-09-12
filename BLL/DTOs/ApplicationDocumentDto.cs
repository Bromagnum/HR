using System.ComponentModel.DataAnnotations;
using DAL.Entities;

namespace BLL.DTOs;

public class ApplicationDocumentDto
{
    public int Id { get; set; }
    public int JobApplicationId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public DocumentType DocumentType { get; set; }
    public string DocumentTypeText { get; set; } = string.Empty;
    public long FileSizeBytes { get; set; }
    public string FileSizeText { get; set; } = string.Empty;
    public string MimeType { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime UploadDate { get; set; }
    public int? UploadedById { get; set; }
    public string? UploadedByName { get; set; }
    public bool IsVerified { get; set; }
    public int? VerifiedById { get; set; }
    public string? VerifiedByName { get; set; }
    public DateTime? VerifiedAt { get; set; }
    public string? VerificationNotes { get; set; }
    public int DownloadCount { get; set; }
    public DateTime? LastDownloadedAt { get; set; }
    public string? LastDownloadedByName { get; set; }
    public string FileExtension { get; set; } = string.Empty;
    public bool IsImageFile { get; set; }
    public bool IsPdfFile { get; set; }
    public bool IsDocumentFile { get; set; }
    public bool IsActive { get; set; }
}

public class ApplicationDocumentCreateDto
{
    [Required(ErrorMessage = "Başvuru seçilmelidir")]
    public int JobApplicationId { get; set; }
    
    [Required(ErrorMessage = "Dosya adı zorunludur")]
    [StringLength(200, ErrorMessage = "Dosya adı en fazla 200 karakter olabilir")]
    public string FileName { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Dosya yolu zorunludur")]
    [StringLength(500, ErrorMessage = "Dosya yolu en fazla 500 karakter olabilir")]
    public string FilePath { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Doküman türü seçilmelidir")]
    public DocumentType DocumentType { get; set; } = DocumentType.Other;
    
    [Required(ErrorMessage = "Dosya boyutu zorunludur")]
    [Range(1, long.MaxValue, ErrorMessage = "Dosya boyutu pozitif olmalıdır")]
    public long FileSizeBytes { get; set; }
    
    [Required(ErrorMessage = "MIME türü zorunludur")]
    [StringLength(50, ErrorMessage = "MIME türü en fazla 50 karakter olabilir")]
    public string MimeType { get; set; } = string.Empty;
    
    [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir")]
    public string? Description { get; set; }
    
    public int? UploadedById { get; set; }
}

public class ApplicationDocumentUpdateDto
{
    public int Id { get; set; }
    
    [StringLength(500)]
    public string? Description { get; set; }
    
    public DocumentType DocumentType { get; set; }
}

public class DocumentVerificationDto
{
    public int Id { get; set; }
    
    public bool IsVerified { get; set; }
    
    [StringLength(1000, ErrorMessage = "Doğrulama notu en fazla 1000 karakter olabilir")]
    public string? VerificationNotes { get; set; }
    
    public int VerifiedById { get; set; }
}

public class DocumentStatisticsDto
{
    public int TotalDocuments { get; set; }
    public int VerifiedDocuments { get; set; }
    public int UnverifiedDocuments { get; set; }
    public long TotalFileSize { get; set; }
    public string TotalFileSizeText { get; set; } = string.Empty;
    public int TotalDownloads { get; set; }
    public Dictionary<string, int> DocumentsByType { get; set; } = new();
    public Dictionary<string, int> DocumentsByExtension { get; set; } = new();
    public List<PopularDocumentDto> MostDownloaded { get; set; } = new();
}

public class PopularDocumentDto
{
    public int Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string DocumentTypeText { get; set; } = string.Empty;
    public string CandidateName { get; set; } = string.Empty;
    public int DownloadCount { get; set; }
}
