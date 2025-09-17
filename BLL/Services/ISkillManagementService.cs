using BLL.DTOs;
using BLL.Utilities;

namespace BLL.Services;

public interface ISkillManagementService
{
    // Skill Template Management
    Task<Result<IEnumerable<SkillTemplateListDto>>> GetAllSkillTemplatesAsync();
    Task<Result<SkillTemplateDetailDto>> GetSkillTemplateByIdAsync(int id);
    Task<Result<SkillTemplateDetailDto>> CreateSkillTemplateAsync(SkillTemplateCreateDto dto);
    Task<Result<SkillTemplateDetailDto>> UpdateSkillTemplateAsync(SkillTemplateUpdateDto dto);
    Task<Result<bool>> DeleteSkillTemplateAsync(int id);
    Task<Result<IEnumerable<SkillTemplateListDto>>> GetFilteredSkillTemplatesAsync(SkillTemplateFilterDto filter);
    Task<Result<IEnumerable<string>>> GetSkillCategoriesAsync();
    Task<Result<IEnumerable<SkillTemplateListDto>>> GetMostUsedSkillsAsync(int count = 10);

    // Person Skill Management
    Task<Result<IEnumerable<PersonSkillDto>>> GetPersonSkillsAsync(int personId);
    Task<Result<PersonSkillDto>> GetPersonSkillByIdAsync(int id);
    Task<Result<PersonSkillDto>> AddPersonSkillAsync(PersonSkillCreateDto dto);
    Task<Result<PersonSkillDto>> UpdatePersonSkillAsync(PersonSkillUpdateDto dto);
    Task<Result<bool>> DeletePersonSkillAsync(int id);
    Task<Result<bool>> EndorsePersonSkillAsync(int personSkillId, int endorsedById, string? notes = null);
    Task<Result<bool>> AssessPersonSkillAsync(int personSkillId, int assessedById);

    // Skill Assessment Management
    Task<Result<IEnumerable<SkillAssessmentDto>>> GetSkillAssessmentsAsync(int personSkillId);
    Task<Result<SkillAssessmentDto>> CreateSkillAssessmentAsync(SkillAssessmentCreateDto dto);
    Task<Result<IEnumerable<SkillAssessmentDto>>> GetPersonAssessmentsAsync(int personId);
    Task<Result<IEnumerable<SkillAssessmentDto>>> GetAssessorAssessmentsAsync(int assessorId);

    // Skill Matching and Analysis
    Task<Result<IEnumerable<PersonSkillDto>>> FindSkillExpertsAsync(int skillTemplateId, int minLevel = 3);
    Task<Result<IEnumerable<PersonSkillDto>>> FindPersonsWithSkillAsync(int skillTemplateId);
    Task<Result<decimal>> CalculateSkillMatchPercentageAsync(int personId, int jobDefinitionId);
    Task<Result<IEnumerable<PersonSkillDto>>> GetSkillGapsAsync(int personId, int jobDefinitionId);

    // Certification Management
    Task<Result<IEnumerable<PersonSkillDto>>> GetCertifiedSkillsAsync(int? personId = null);
    Task<Result<IEnumerable<PersonSkillDto>>> GetExpiringCertificationsAsync(int daysFromNow = 90);
    Task<Result<IEnumerable<PersonSkillDto>>> GetExpiredCertificationsAsync();
    Task<Result<bool>> RenewCertificationAsync(int personSkillId, DateTime newExpiryDate);

    // Statistics and Reporting
    Task<Result<SkillAnalyticsDto>> GetSkillAnalyticsAsync();
    Task<Result<SkillSummaryDto>> GetSkillSummaryAsync();
    Task<Result<IEnumerable<SkillCategorySummaryDto>>> GetSkillCategorySummaryAsync();
    Task<Result<IEnumerable<SkillTypeSummaryDto>>> GetSkillTypeSummaryAsync();
    Task<Result<IEnumerable<PersonSkillDto>>> GetTopSkillsInOrganizationAsync(int count = 10);

    // Search and Filtering
    Task<Result<IEnumerable<SkillTemplateListDto>>> SearchSkillTemplatesAsync(string searchTerm);
    Task<Result<IEnumerable<PersonSkillDto>>> SearchPersonSkillsAsync(string searchTerm);
    Task<Result<IEnumerable<PersonSkillDto>>> GetFilteredPersonSkillsAsync(PersonSkillFilterDto filter);

    // Bulk Operations
    Task<Result<bool>> BulkAddPersonSkillsAsync(int personId, List<PersonSkillCreateDto> skills);
    Task<Result<bool>> BulkAssessPersonSkillsAsync(int personId, int assessorId, List<SkillAssessmentCreateDto> assessments);
    Task<Result<bool>> ImportSkillTemplatesAsync(List<SkillTemplateCreateDto> templates);

    // Export
    Task<Result<byte[]>> ExportPersonSkillsAsync(int personId);
    Task<Result<byte[]>> ExportSkillSummaryAsync();
    Task<Result<byte[]>> ExportSkillTemplatesAsync();
}
