using System.ComponentModel.DataAnnotations;
using DAL.Entities;

namespace BLL.DTOs;

public class InterviewNoteDto
{
    public int Id { get; set; }
    public int JobApplicationId { get; set; }
    public int? CandidateId { get; set; }
    public string? CandidateName { get; set; }
    public int InterviewerId { get; set; }
    public string InterviewerName { get; set; } = string.Empty;
    public DateTime InterviewDate { get; set; }
    public InterviewType InterviewType { get; set; }
    public string InterviewTypeText { get; set; } = string.Empty;
    public int? DurationMinutes { get; set; }
    public string Duration { get; set; } = string.Empty;
    public string? Location { get; set; }
    public string? MeetingLink { get; set; }
    
    // Scores
    public int? TechnicalSkillsScore { get; set; }
    public int? CommunicationScore { get; set; }
    public int? ProblemSolvingScore { get; set; }
    public int? CulturalFitScore { get; set; }
    public int? MotivationScore { get; set; }
    public int? OverallScore { get; set; }
    public decimal? AverageScore { get; set; }
    
    // Notes
    public string? TechnicalNotes { get; set; }
    public string? BehavioralNotes { get; set; }
    public string? StrengthsNotes { get; set; }
    public string? WeaknessesNotes { get; set; }
    public string? GeneralNotes { get; set; }
    public string? QuestionsAsked { get; set; }
    public string? CandidateQuestions { get; set; }
    
    // Decision
    public string? Recommendation { get; set; }
    public string? RecommendationReason { get; set; }
    public string? NextSteps { get; set; }
    public DateTime? FollowUpDate { get; set; }
    
    // Additional Info
    public string? AttendeesList { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string? InternalComments { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }
}

public class InterviewNoteCreateDto
{
    [Required(ErrorMessage = "Başvuru seçilmelidir")]
    public int JobApplicationId { get; set; }
    
    public int? CandidateId { get; set; }
    
    [Required(ErrorMessage = "Mülakatçı seçilmelidir")]
    public int InterviewerId { get; set; }
    
    [Required(ErrorMessage = "Mülakat tarihi zorunludur")]
    public DateTime InterviewDate { get; set; }
    
    [Required(ErrorMessage = "Mülakat türü seçilmelidir")]
    public InterviewType InterviewType { get; set; } = InterviewType.InPerson;
    
    [Range(30, 480, ErrorMessage = "Süre 30 dakika - 8 saat arasında olmalıdır")]
    public int? DurationMinutes { get; set; }
    
    [StringLength(200)]
    public string? Location { get; set; }
    
    [StringLength(500)]
    [Url(ErrorMessage = "Geçerli bir URL giriniz")]
    public string? MeetingLink { get; set; }
    
    // Scores
    [Range(1, 10, ErrorMessage = "Puan 1-10 arasında olmalıdır")]
    public int? TechnicalSkillsScore { get; set; }
    
    [Range(1, 10, ErrorMessage = "Puan 1-10 arasında olmalıdır")]
    public int? CommunicationScore { get; set; }
    
    [Range(1, 10, ErrorMessage = "Puan 1-10 arasında olmalıdır")]
    public int? ProblemSolvingScore { get; set; }
    
    [Range(1, 10, ErrorMessage = "Puan 1-10 arasında olmalıdır")]
    public int? CulturalFitScore { get; set; }
    
    [Range(1, 10, ErrorMessage = "Puan 1-10 arasında olmalıdır")]
    public int? MotivationScore { get; set; }
    
    [Range(1, 10, ErrorMessage = "Puan 1-10 arasında olmalıdır")]
    public int? OverallScore { get; set; }
    
    // Notes
    [StringLength(2000)]
    public string? TechnicalNotes { get; set; }
    
    [StringLength(2000)]
    public string? BehavioralNotes { get; set; }
    
    [StringLength(2000)]
    public string? StrengthsNotes { get; set; }
    
    [StringLength(2000)]
    public string? WeaknessesNotes { get; set; }
    
    [StringLength(2000)]
    public string? GeneralNotes { get; set; }
    
    [StringLength(1000)]
    public string? QuestionsAsked { get; set; }
    
    [StringLength(1000)]
    public string? CandidateQuestions { get; set; }
    
    // Decision
    [StringLength(50)]
    public string? Recommendation { get; set; }
    
    [StringLength(1000)]
    public string? RecommendationReason { get; set; }
    
    [StringLength(1000)]
    public string? NextSteps { get; set; }
    
    public DateTime? FollowUpDate { get; set; }
    
    // Additional Info
    [StringLength(500)]
    public string? AttendeesList { get; set; }
    
    public bool IsCompleted { get; set; }
    
    [StringLength(1000)]
    public string? InternalComments { get; set; }
}

public class InterviewNoteUpdateDto : InterviewNoteCreateDto
{
    public int Id { get; set; }
    
    public DateTime? CompletedAt { get; set; }
}

public class InterviewScheduleDto
{
    public int JobApplicationId { get; set; }
    public string CandidateName { get; set; } = string.Empty;
    public string PositionName { get; set; } = string.Empty;
    public DateTime InterviewDate { get; set; }
    public string InterviewerName { get; set; } = string.Empty;
    public InterviewType InterviewType { get; set; }
    public string InterviewTypeText { get; set; } = string.Empty;
    public string? Location { get; set; }
    public string? MeetingLink { get; set; }
    public bool IsCompleted { get; set; }
}
