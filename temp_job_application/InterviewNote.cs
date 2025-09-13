using System.ComponentModel.DataAnnotations;

namespace DAL.Entities;

public enum InterviewType
{
    Phone = 1,
    Video = 2,
    InPerson = 3,
    Technical = 4,
    HR = 5,
    Panel = 6,
    Final = 7
}

public class InterviewNote : BaseEntity
{
    [Required]
    public int JobApplicationId { get; set; }
    public virtual JobApplication JobApplication { get; set; } = null!;
    
    public int? CandidateId { get; set; }
    public virtual Candidate? Candidate { get; set; }
    
    [Required]
    public int InterviewerId { get; set; }
    public virtual ApplicationUser Interviewer { get; set; } = null!;
    
    [Required]
    public DateTime InterviewDate { get; set; }
    
    [Required]
    public InterviewType InterviewType { get; set; } = InterviewType.InPerson;
    
    [Range(30, 480)] // 30 dakika - 8 saat
    public int? DurationMinutes { get; set; }
    
    [StringLength(200)]
    public string? Location { get; set; }
    
    [StringLength(500)]
    public string? MeetingLink { get; set; }
    
    // Değerlendirme Kriterleri
    [Range(1, 10)]
    public int? TechnicalSkillsScore { get; set; }
    
    [Range(1, 10)]
    public int? CommunicationScore { get; set; }
    
    [Range(1, 10)]
    public int? ProblemSolvingScore { get; set; }
    
    [Range(1, 10)]
    public int? CulturalFitScore { get; set; }
    
    [Range(1, 10)]
    public int? MotivationScore { get; set; }
    
    [Range(1, 10)]
    public int? OverallScore { get; set; }
    
    // Notlar
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
    
    // Karar
    [StringLength(50)]
    public string? Recommendation { get; set; } // Proceed, Reject, Hold, SecondInterview
    
    [StringLength(1000)]
    public string? RecommendationReason { get; set; }
    
    // Sonraki Adımlar
    [StringLength(1000)]
    public string? NextSteps { get; set; }
    
    public DateTime? FollowUpDate { get; set; }
    
    // Ek Bilgiler
    [StringLength(500)]
    public string? AttendeesList { get; set; } // Diğer katılımcılar
    
    public bool IsCompleted { get; set; } = false;
    
    public DateTime? CompletedAt { get; set; }
    
    [StringLength(1000)]
    public string? InternalComments { get; set; }
    
    // Computed Properties
    public string InterviewTypeText => InterviewType switch
    {
        InterviewType.Phone => "Telefon Görüşmesi",
        InterviewType.Video => "Video Görüşmesi",
        InterviewType.InPerson => "Yüz Yüze",
        InterviewType.Technical => "Teknik Mülakat",
        InterviewType.HR => "İK Mülakatı",
        InterviewType.Panel => "Panel Mülakatı",
        InterviewType.Final => "Final Mülakatı",
        _ => "Bilinmiyor"
    };
    
    public string Duration => DurationMinutes.HasValue ? 
        $"{DurationMinutes.Value / 60}s {DurationMinutes.Value % 60}dk" : 
        "Belirtilmemiş";
    
    public decimal? AverageScore
    {
        get
        {
            var scores = new List<int?> 
            { 
                TechnicalSkillsScore, CommunicationScore, ProblemSolvingScore, 
                CulturalFitScore, MotivationScore 
            };
            
            var validScores = scores.Where(s => s.HasValue).Select(s => s!.Value).ToList();
            
            return validScores.Any() ? Math.Round((decimal)validScores.Average(), 1) : null;
        }
    }
}
