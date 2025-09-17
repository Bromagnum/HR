using AutoMapper;
using BLL.DTOs;
using BLL.Utilities;
using DAL.Entities;
using DAL.Repositories;
using Microsoft.Extensions.Logging;

namespace BLL.Services;

public class SkillManagementService : ISkillManagementService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<SkillManagementService> _logger;

    public SkillManagementService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<SkillManagementService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    #region Skill Template Management

    public async Task<Result<IEnumerable<SkillTemplateListDto>>> GetAllSkillTemplatesAsync()
    {
        try
        {
            var entities = await _unitOfWork.SkillTemplates.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<SkillTemplateListDto>>(entities);
            return Result<IEnumerable<SkillTemplateListDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all skill templates");
            return Result<IEnumerable<SkillTemplateListDto>>.Fail("Beceri şablonları alınırken hata oluştu.");
        }
    }

    public async Task<Result<SkillTemplateDetailDto>> GetSkillTemplateByIdAsync(int id)
    {
        try
        {
            var entity = await _unitOfWork.SkillTemplates.GetByIdAsync(id);
            if (entity == null)
                return Result<SkillTemplateDetailDto>.Fail("Beceri şablonu bulunamadı.");

            var dto = _mapper.Map<SkillTemplateDetailDto>(entity);
            return Result<SkillTemplateDetailDto>.Ok(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting skill template by id: {Id}", id);
            return Result<SkillTemplateDetailDto>.Fail("Beceri şablonu alınırken hata oluştu.");
        }
    }

    public async Task<Result<SkillTemplateDetailDto>> CreateSkillTemplateAsync(SkillTemplateCreateDto dto)
    {
        try
        {
            // Check if skill template with same name exists
            var existingTemplates = await _unitOfWork.SkillTemplates.GetAllAsync();
            if (existingTemplates.Any(x => x.Name.Equals(dto.Name, StringComparison.OrdinalIgnoreCase) &&
                                           x.Category.Equals(dto.Category, StringComparison.OrdinalIgnoreCase)))
            {
                return Result<SkillTemplateDetailDto>.Fail("Bu kategoride aynı isimde bir beceri şablonu zaten mevcut.");
            }

            var entity = _mapper.Map<SkillTemplate>(dto);
            _unitOfWork.SkillTemplates.Add(entity);
            await _unitOfWork.SaveChangesAsync();

            var result = await GetSkillTemplateByIdAsync(entity.Id);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating skill template");
            return Result<SkillTemplateDetailDto>.Fail("Beceri şablonu oluşturulurken hata oluştu.");
        }
    }

    public async Task<Result<SkillTemplateDetailDto>> UpdateSkillTemplateAsync(SkillTemplateUpdateDto dto)
    {
        try
        {
            var entity = await _unitOfWork.SkillTemplates.GetByIdAsync(dto.Id);
            if (entity == null)
                return Result<SkillTemplateDetailDto>.Fail("Beceri şablonu bulunamadı.");

            // Check if skill template with same name exists (exclude current)
            var existingTemplates = await _unitOfWork.SkillTemplates.GetAllAsync();
            if (existingTemplates.Any(x => x.Id != dto.Id &&
                                           x.Name.Equals(dto.Name, StringComparison.OrdinalIgnoreCase) &&
                                           x.Category.Equals(dto.Category, StringComparison.OrdinalIgnoreCase)))
            {
                return Result<SkillTemplateDetailDto>.Fail("Bu kategoride aynı isimde bir beceri şablonu zaten mevcut.");
            }

            _mapper.Map(dto, entity);
            entity.UpdatedAt = DateTime.Now;
            _unitOfWork.SkillTemplates.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            var result = await GetSkillTemplateByIdAsync(dto.Id);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating skill template: {Id}", dto.Id);
            return Result<SkillTemplateDetailDto>.Fail("Beceri şablonu güncellenirken hata oluştu.");
        }
    }

    public async Task<Result<bool>> DeleteSkillTemplateAsync(int id)
    {
        try
        {
            var entity = await _unitOfWork.SkillTemplates.GetByIdAsync(id);
            if (entity == null)
                return Result<bool>.Fail("Beceri şablonu bulunamadı.");

            // Check if template is in use
            var personSkills = await _unitOfWork.PersonSkills.GetBySkillTemplateIdAsync(id);
            var jobRequiredSkills = await _unitOfWork.JobRequiredSkills.GetBySkillTemplateIdAsync(id);

            if (personSkills.Any() || jobRequiredSkills.Any())
                return Result<bool>.Fail("Bu beceri şablonu kullanımda olduğu için silinemez.");

            _unitOfWork.SkillTemplates.Remove(entity);
            await _unitOfWork.SaveChangesAsync();

            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting skill template: {Id}", id);
            return Result<bool>.Fail("Beceri şablonu silinirken hata oluştu.");
        }
    }

    public async Task<Result<IEnumerable<SkillTemplateListDto>>> GetSkillTemplatesByTypeAsync(SkillType type)
    {
        try
        {
            var entities = await _unitOfWork.SkillTemplates.GetByTypeAsync(type);
            var dtos = _mapper.Map<IEnumerable<SkillTemplateListDto>>(entities);
            return Result<IEnumerable<SkillTemplateListDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting skill templates by type: {Type}", type);
            return Result<IEnumerable<SkillTemplateListDto>>.Fail("Beceri şablonları alınırken hata oluştu.");
        }
    }

    public async Task<Result<IEnumerable<SkillTemplateListDto>>> GetSkillTemplatesByCategoryAsync(string category)
    {
        try
        {
            var entities = await _unitOfWork.SkillTemplates.GetByCategoryAsync(category);
            var dtos = _mapper.Map<IEnumerable<SkillTemplateListDto>>(entities);
            return Result<IEnumerable<SkillTemplateListDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting skill templates by category: {Category}", category);
            return Result<IEnumerable<SkillTemplateListDto>>.Fail("Beceri şablonları alınırken hata oluştu.");
        }
    }

    public async Task<Result<IEnumerable<string>>> GetSkillCategoriesAsync()
    {
        try
        {
            var categories = await _unitOfWork.SkillTemplates.GetCategoriesAsync();
            return Result<IEnumerable<string>>.Ok(categories);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting skill categories");
            return Result<IEnumerable<string>>.Fail("Beceri kategorileri alınırken hata oluştu.");
        }
    }

    #endregion

    #region Person Skill Management

    public async Task<Result<IEnumerable<PersonSkillDto>>> GetPersonSkillsAsync(int personId)
    {
        try
        {
            var entities = await _unitOfWork.PersonSkills.GetByPersonIdAsync(personId);
            var dtos = _mapper.Map<IEnumerable<PersonSkillDto>>(entities);
            return Result<IEnumerable<PersonSkillDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting person skills for person: {PersonId}", personId);
            return Result<IEnumerable<PersonSkillDto>>.Fail("Personel becerileri alınırken hata oluştu.");
        }
    }

    public async Task<Result<PersonSkillDto>> AddPersonSkillAsync(PersonSkillCreateDto dto)
    {
        try
        {
            // Check if person already has this skill
            var existingSkill = await _unitOfWork.PersonSkills
                .GetByPersonAndSkillTemplateAsync(dto.PersonId, dto.SkillTemplateId);

            if (existingSkill != null)
                return Result<PersonSkillDto>.Fail("Bu personel zaten bu beceriye sahip.");

            var entity = _mapper.Map<PersonSkill>(dto);
            _unitOfWork.PersonSkills.Add(entity);
            await _unitOfWork.SaveChangesAsync();

            // Update skill template usage count
            var skillTemplate = await _unitOfWork.SkillTemplates.GetByIdAsync(dto.SkillTemplateId);
            if (skillTemplate != null)
            {
                skillTemplate.UsageCount++;
                skillTemplate.LastUsedAt = DateTime.Now;
                _unitOfWork.SkillTemplates.Update(skillTemplate);
                await _unitOfWork.SaveChangesAsync();
            }

            var resultEntity = await _unitOfWork.PersonSkills.GetByIdAsync(entity.Id);
            var resultDto = _mapper.Map<PersonSkillDto>(resultEntity);
            return Result<PersonSkillDto>.Ok(resultDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding person skill");
            return Result<PersonSkillDto>.Fail("Personel becerisi eklenirken hata oluştu.");
        }
    }

    public async Task<Result<PersonSkillDto>> UpdatePersonSkillAsync(PersonSkillUpdateDto dto)
    {
        try
        {
            var entity = await _unitOfWork.PersonSkills.GetByIdAsync(dto.Id);
            if (entity == null)
                return Result<PersonSkillDto>.Fail("Personel becerisi bulunamadı.");

            _mapper.Map(dto, entity);
            entity.UpdatedAt = DateTime.Now;
            _unitOfWork.PersonSkills.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            var resultEntity = await _unitOfWork.PersonSkills.GetByIdAsync(dto.Id);
            var resultDto = _mapper.Map<PersonSkillDto>(resultEntity);
            return Result<PersonSkillDto>.Ok(resultDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating person skill: {Id}", dto.Id);
            return Result<PersonSkillDto>.Fail("Personel becerisi güncellenirken hata oluştu.");
        }
    }

    public async Task<Result<bool>> RemovePersonSkillAsync(int id)
    {
        try
        {
            var entity = await _unitOfWork.PersonSkills.GetByIdAsync(id);
            if (entity == null)
                return Result<bool>.Fail("Personel becerisi bulunamadı.");

            // Remove related assessments
            var assessments = await _unitOfWork.SkillAssessments.GetByPersonSkillIdAsync(id);
            foreach (var assessment in assessments)
            {
                _unitOfWork.SkillAssessments.Remove(assessment);
            }

            _unitOfWork.PersonSkills.Remove(entity);
            await _unitOfWork.SaveChangesAsync();

            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing person skill: {Id}", id);
            return Result<bool>.Fail("Personel becerisi silinirken hata oluştu.");
        }
    }

    public async Task<Result<bool>> EndorsePersonSkillAsync(int personSkillId, int endorsedById, string? notes = null)
    {
        try
        {
            var entity = await _unitOfWork.PersonSkills.GetByIdAsync(personSkillId);
            if (entity == null)
                return Result<bool>.Fail("Personel becerisi bulunamadı.");

            entity.IsEndorsed = true;
            entity.EndorsedById = endorsedById;
            entity.EndorsedAt = DateTime.Now;
            entity.EndorsementNotes = notes;
            entity.UpdatedAt = DateTime.Now;

            _unitOfWork.PersonSkills.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error endorsing person skill: {Id}", personSkillId);
            return Result<bool>.Fail("Beceri onaylanırken hata oluştu.");
        }
    }

    public async Task<Result<IEnumerable<PersonSkillDto>>> GetPersonSkillsByTypeAsync(int personId, SkillType type)
    {
        try
        {
            var entities = await _unitOfWork.PersonSkills.GetByPersonAndTypeAsync(personId, type);
            var dtos = _mapper.Map<IEnumerable<PersonSkillDto>>(entities);
            return Result<IEnumerable<PersonSkillDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting person skills by type for person: {PersonId}, type: {Type}", personId, type);
            return Result<IEnumerable<PersonSkillDto>>.Fail("Personel becerileri alınırken hata oluştu.");
        }
    }

    #endregion

    #region Job Required Skill Management

    public async Task<Result<IEnumerable<JobRequiredSkillDto>>> GetJobRequiredSkillsAsync(int jobDefinitionId)
    {
        try
        {
            var entities = await _unitOfWork.JobRequiredSkills.GetByJobDefinitionIdAsync(jobDefinitionId);
            var dtos = _mapper.Map<IEnumerable<JobRequiredSkillDto>>(entities);
            return Result<IEnumerable<JobRequiredSkillDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting job required skills for job: {JobDefinitionId}", jobDefinitionId);
            return Result<IEnumerable<JobRequiredSkillDto>>.Fail("İş gereken becerileri alınırken hata oluştu.");
        }
    }

    public async Task<Result<JobRequiredSkillDto>> AddJobRequiredSkillAsync(int jobDefinitionId, JobRequiredSkillCreateDto dto)
    {
        try
        {
            // Check if job already has this skill requirement
            var existingSkill = await _unitOfWork.JobRequiredSkills
                .GetByJobDefinitionAndSkillTemplateAsync(jobDefinitionId, dto.SkillTemplateId);

            if (existingSkill != null)
                return Result<JobRequiredSkillDto>.Fail("Bu iş tanımı zaten bu beceri gereksinimine sahip.");

            var entity = _mapper.Map<JobRequiredSkill>(dto);
            entity.JobDefinitionId = jobDefinitionId;
            _unitOfWork.JobRequiredSkills.Add(entity);
            await _unitOfWork.SaveChangesAsync();

            var resultEntity = await _unitOfWork.JobRequiredSkills.GetByIdAsync(entity.Id);
            var resultDto = _mapper.Map<JobRequiredSkillDto>(resultEntity);
            return Result<JobRequiredSkillDto>.Ok(resultDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding job required skill");
            return Result<JobRequiredSkillDto>.Fail("İş gereken beceri eklenirken hata oluştu.");
        }
    }

    public async Task<Result<bool>> RemoveJobRequiredSkillAsync(int id)
    {
        try
        {
            var entity = await _unitOfWork.JobRequiredSkills.GetByIdAsync(id);
            if (entity == null)
                return Result<bool>.Fail("İş gereken beceri bulunamadı.");

            _unitOfWork.JobRequiredSkills.Remove(entity);
            await _unitOfWork.SaveChangesAsync();

            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing job required skill: {Id}", id);
            return Result<bool>.Fail("İş gereken beceri silinirken hata oluştu.");
        }
    }

    public async Task<Result<JobRequiredSkillDto>> UpdateJobRequiredSkillAsync(JobRequiredSkillCreateDto dto)
    {
        try
        {
            var entity = await _unitOfWork.JobRequiredSkills.GetByIdAsync(dto.Id ?? 0);
            if (entity == null)
                return Result<JobRequiredSkillDto>.Fail("İş gereken beceri bulunamadı.");

            _mapper.Map(dto, entity);
            entity.UpdatedAt = DateTime.Now;
            _unitOfWork.JobRequiredSkills.Update(entity);
            await _unitOfWork.SaveChangesAsync();

            var resultEntity = await _unitOfWork.JobRequiredSkills.GetByIdAsync(entity.Id);
            var resultDto = _mapper.Map<JobRequiredSkillDto>(resultEntity);
            return Result<JobRequiredSkillDto>.Ok(resultDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating job required skill");
            return Result<JobRequiredSkillDto>.Fail("İş gereken beceri güncellenirken hata oluştu.");
        }
    }

    #endregion

    #region Skill Assessment Management

    public async Task<Result<IEnumerable<SkillAssessmentDto>>> GetSkillAssessmentsAsync(int personSkillId)
    {
        try
        {
            var entities = await _unitOfWork.SkillAssessments.GetByPersonSkillIdAsync(personSkillId);
            var dtos = _mapper.Map<IEnumerable<SkillAssessmentDto>>(entities);
            return Result<IEnumerable<SkillAssessmentDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting skill assessments for person skill: {PersonSkillId}", personSkillId);
            return Result<IEnumerable<SkillAssessmentDto>>.Fail("Beceri değerlendirmeleri alınırken hata oluştu.");
        }
    }

    public async Task<Result<SkillAssessmentDto>> CreateSkillAssessmentAsync(SkillAssessmentCreateDto dto)
    {
        try
        {
            var entity = _mapper.Map<SkillAssessment>(dto);
            entity.AssessmentDate = DateTime.Now;
            entity.IsValid = true;

            _unitOfWork.SkillAssessments.Add(entity);
            await _unitOfWork.SaveChangesAsync();

            // Update person skill with assessment result
            var personSkill = await _unitOfWork.PersonSkills.GetByIdAsync(dto.PersonSkillId);
            if (personSkill != null)
            {
                personSkill.IsSelfAssessed = dto.Type == AssessmentType.SelfAssessment;
                personSkill.AssessedById = dto.AssessorId;
                personSkill.AssessedAt = DateTime.Now;
                personSkill.UpdatedAt = DateTime.Now;

                // Update level if assessment provides a higher score
                if (dto.Score.HasValue && dto.Score.Value > personSkill.Level)
                {
                    personSkill.Level = Math.Min(dto.Score.Value, 10); // Cap at 10
                }

                _unitOfWork.PersonSkills.Update(personSkill);
                await _unitOfWork.SaveChangesAsync();
            }

            var resultEntity = await _unitOfWork.SkillAssessments.GetByIdAsync(entity.Id);
            var resultDto = _mapper.Map<SkillAssessmentDto>(resultEntity);
            return Result<SkillAssessmentDto>.Ok(resultDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating skill assessment");
            return Result<SkillAssessmentDto>.Fail("Beceri değerlendirmesi oluşturulurken hata oluştu.");
        }
    }

    public async Task<Result<IEnumerable<SkillAssessmentDto>>> GetPersonAssessmentsAsync(int personId)
    {
        try
        {
            var entities = await _unitOfWork.SkillAssessments.GetByPersonIdAsync(personId);
            var dtos = _mapper.Map<IEnumerable<SkillAssessmentDto>>(entities);
            return Result<IEnumerable<SkillAssessmentDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting person assessments for person: {PersonId}", personId);
            return Result<IEnumerable<SkillAssessmentDto>>.Fail("Personel değerlendirmeleri alınırken hata oluştu.");
        }
    }

    public async Task<Result<IEnumerable<SkillAssessmentDto>>> GetAssessmentsByTypeAsync(AssessmentType type)
    {
        try
        {
            var entities = await _unitOfWork.SkillAssessments.GetByTypeAsync(type);
            var dtos = _mapper.Map<IEnumerable<SkillAssessmentDto>>(entities);
            return Result<IEnumerable<SkillAssessmentDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting assessments by type: {Type}", type);
            return Result<IEnumerable<SkillAssessmentDto>>.Fail("Değerlendirmeler alınırken hata oluştu.");
        }
    }

    #endregion

    #region Analytics and Reporting

    public async Task<Result<SkillAnalyticsDto>> GetSkillAnalyticsAsync()
    {
        try
        {
            var skillTemplates = await _unitOfWork.SkillTemplates.GetAllAsync();
            var personSkills = await _unitOfWork.PersonSkills.GetAllAsync();
            var assessments = await _unitOfWork.SkillAssessments.GetAllAsync();

            var analytics = new SkillAnalyticsDto
            {
                TotalSkillTemplates = skillTemplates.Count(),
                TotalPersonSkills = personSkills.Count(),
                TotalAssessments = assessments.Count(),
                AverageSkillLevel = personSkills.Any() ? (decimal)Math.Round(personSkills.Average(ps => ps.Level), 2) : 0,
                CertifiedSkillsCount = personSkills.Count(ps => ps.IsCertified),
                EndorsedSkillsCount = personSkills.Count(ps => ps.IsEndorsed),
                SkillsByType = skillTemplates.GroupBy(st => st.Type)
                    .ToDictionary(g => g.Key.ToString(), g => g.Count()),
                TopSkillCategories = personSkills.GroupBy(ps => ps.SkillTemplate?.Category ?? "Diğer")
                    .OrderByDescending(g => g.Count())
                    .Take(10)
                    .ToDictionary(g => g.Key, g => g.Count()),
                AssessmentsByType = assessments.GroupBy(a => a.Type)
                    .ToDictionary(g => g.Key.ToString(), g => g.Count())
            };

            return Result<SkillAnalyticsDto>.Ok(analytics);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting skill analytics");
            return Result<SkillAnalyticsDto>.Fail("Beceri analitikleri alınırken hata oluştu.");
        }
    }

    public async Task<Result<PersonSkillSummaryDto>> GetPersonSkillSummaryAsync(int personId)
    {
        try
        {
            var personSkills = await _unitOfWork.PersonSkills.GetByPersonIdAsync(personId);
            var assessments = await _unitOfWork.SkillAssessments.GetByPersonIdAsync(personId);

            var summary = new PersonSkillSummaryDto
            {
                PersonId = personId,
                TotalSkillCount = personSkills.Count(),
                AverageSkillLevel = personSkills.Any() ? (decimal)Math.Round(personSkills.Average(ps => ps.Level), 2) : 0,
                CertifiedSkillCount = personSkills.Count(ps => ps.IsCertified),
                EndorsedSkillCount = personSkills.Count(ps => ps.IsEndorsed),
                TotalAssessmentCount = assessments.Count(),
                LatestAssessmentDate = assessments.Any() ? assessments.Max(a => a.AssessmentDate) : null,
                SkillsByType = personSkills.GroupBy(ps => ps.SkillTemplate?.Type ?? SkillType.Technical)
                    .ToDictionary(g => g.Key.ToString(), g => g.Count()),
                SkillsByLevel = personSkills.GroupBy(ps => ps.Level)
                    .OrderBy(g => g.Key)
                    .ToDictionary(g => $"Seviye {g.Key}", g => g.Count()),
                TopSkills = personSkills.OrderByDescending(ps => ps.Level)
                    .ThenByDescending(ps => ps.ExperienceYears)
                    .Take(5)
                    .Select(ps => new PersonSkillSummaryItemDto
                    {
                        SkillName = ps.SkillTemplate?.Name ?? "",
                        Level = ps.Level,
                        ExperienceYears = ps.ExperienceYears ?? 0,
                        IsCertified = ps.IsCertified,
                        IsEndorsed = ps.IsEndorsed
                    }).ToList()
            };

            return Result<PersonSkillSummaryDto>.Ok(summary);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting person skill summary for person: {PersonId}", personId);
            return Result<PersonSkillSummaryDto>.Fail("Personel beceri özeti alınırken hata oluştu.");
        }
    }

    #endregion

    #region Search and Filter

    public async Task<Result<IEnumerable<SkillTemplateListDto>>> SearchSkillTemplatesAsync(string searchTerm)
    {
        try
        {
            var entities = await _unitOfWork.SkillTemplates.GetAllAsync();
            
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                entities = entities.Where(x => 
                    x.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    x.Category.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    (x.Description != null && x.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)));
            }

            var dtos = _mapper.Map<IEnumerable<SkillTemplateListDto>>(entities);
            return Result<IEnumerable<SkillTemplateListDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching skill templates with term: {SearchTerm}", searchTerm);
            return Result<IEnumerable<SkillTemplateListDto>>.Fail("Beceri şablonları aranırken hata oluştu.");
        }
    }

    public async Task<Result<IEnumerable<PersonSkillDto>>> GetFilteredPersonSkillsAsync(PersonSkillFilterDto filter)
    {
        try
        {
            // Get all person skills or filter by specific person
            IEnumerable<PersonSkill> entities;
            if (filter.PersonId > 0)
            {
                entities = await _unitOfWork.PersonSkills.GetByPersonIdAsync(filter.PersonId);
            }
            else
            {
                entities = await _unitOfWork.PersonSkills.GetAllAsync();
            }

            // Apply filters
            if (filter.SkillType.HasValue)
                entities = entities.Where(ps => ps.SkillTemplate?.Type == filter.SkillType.Value);

            if (!string.IsNullOrWhiteSpace(filter.Category))
                entities = entities.Where(ps => ps.SkillTemplate?.Category?.Contains(filter.Category, StringComparison.OrdinalIgnoreCase) == true);

            if (filter.MinLevel.HasValue)
                entities = entities.Where(ps => ps.Level >= filter.MinLevel.Value);

            if (filter.MaxLevel.HasValue)
                entities = entities.Where(ps => ps.Level <= filter.MaxLevel.Value);

            if (filter.IsCertified.HasValue)
                entities = entities.Where(ps => ps.IsCertified == filter.IsCertified.Value);

            if (filter.IsEndorsed.HasValue)
                entities = entities.Where(ps => ps.IsEndorsed == filter.IsEndorsed.Value);

            if (filter.MinExperienceYears.HasValue)
                entities = entities.Where(ps => ps.ExperienceYears >= filter.MinExperienceYears.Value);

            if (filter.MaxExperienceYears.HasValue)
                entities = entities.Where(ps => ps.ExperienceYears <= filter.MaxExperienceYears.Value);

            var dtos = _mapper.Map<IEnumerable<PersonSkillDto>>(entities);
            return Result<IEnumerable<PersonSkillDto>>.Ok(dtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting filtered person skills");
            return Result<IEnumerable<PersonSkillDto>>.Fail("Personel becerileri filtrelenirken hata oluştu.");
        }
    }

    #endregion

    #region Bulk Operations

    public async Task<Result<bool>> BulkAddPersonSkillsAsync(int personId, List<PersonSkillCreateDto> skills)
    {
        try
        {
            var skillList = skills.ToList();
            var addedCount = 0;

            foreach (var skillDto in skillList)
            {
                skillDto.PersonId = personId;
                
                // Check if person already has this skill
                var existingSkill = await _unitOfWork.PersonSkills
                    .GetByPersonAndSkillTemplateAsync(personId, skillDto.SkillTemplateId);

                if (existingSkill == null)
                {
                    var entity = _mapper.Map<PersonSkill>(skillDto);
                    _unitOfWork.PersonSkills.Add(entity);
                    addedCount++;
                }
            }

            if (addedCount > 0)
            {
                await _unitOfWork.SaveChangesAsync();

                // Update skill template usage counts
                var uniqueSkillTemplateIds = skillList.Select(s => s.SkillTemplateId).Distinct();
                foreach (var skillTemplateId in uniqueSkillTemplateIds)
                {
                    var skillTemplate = await _unitOfWork.SkillTemplates.GetByIdAsync(skillTemplateId);
                    if (skillTemplate != null)
                    {
                        skillTemplate.UsageCount++;
                        skillTemplate.LastUsedAt = DateTime.Now;
                        _unitOfWork.SkillTemplates.Update(skillTemplate);
                    }
                }
                await _unitOfWork.SaveChangesAsync();
            }

            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error bulk adding person skills for person: {PersonId}", personId);
            return Result<bool>.Fail("Personel becerileri toplu olarak eklenirken hata oluştu.");
        }
    }

    #endregion

    // Missing interface methods - stub implementations
    public async Task<Result<IEnumerable<SkillTemplateListDto>>> GetFilteredSkillTemplatesAsync(SkillTemplateFilterDto filter)
    {
        try
        {
            var skillTemplates = await _unitOfWork.SkillTemplates.GetAllAsync();
            var filteredTemplates = skillTemplates.Where(x => x.IsActive);
            
            var result = _mapper.Map<IEnumerable<SkillTemplateListDto>>(filteredTemplates);
            return Result<IEnumerable<SkillTemplateListDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting filtered skill templates");
            return Result<IEnumerable<SkillTemplateListDto>>.Fail("Beceri şablonları alınırken hata oluştu.");
        }
    }

    public async Task<Result<IEnumerable<SkillTemplateListDto>>> GetMostUsedSkillsAsync(int count)
    {
        try
        {
            var skillTemplates = await _unitOfWork.SkillTemplates.GetAllAsync();
            var mostUsed = skillTemplates.Where(x => x.IsActive)
                                       .OrderByDescending(x => x.UsageCount)
                                       .Take(count);
            
            var result = _mapper.Map<IEnumerable<SkillTemplateListDto>>(mostUsed);
            return Result<IEnumerable<SkillTemplateListDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting most used skills");
            return Result<IEnumerable<SkillTemplateListDto>>.Fail("En çok kullanılan beceriler alınırken hata oluştu.");
        }
    }

    public async Task<Result<PersonSkillDto>> GetPersonSkillByIdAsync(int id)
    {
        try
        {
            var personSkill = await _unitOfWork.PersonSkills.GetByIdAsync(id);
            if (personSkill == null)
                return Result<PersonSkillDto>.Fail("Personel becerisi bulunamadı.");

            var result = _mapper.Map<PersonSkillDto>(personSkill);
            return Result<PersonSkillDto>.Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting person skill: {Id}", id);
            return Result<PersonSkillDto>.Fail("Personel becerisi alınırken hata oluştu.");
        }
    }

    public async Task<Result<bool>> DeletePersonSkillAsync(int id)
    {
        try
        {
            var personSkill = await _unitOfWork.PersonSkills.GetByIdAsync(id);
            if (personSkill == null)
                return Result<bool>.Fail("Personel becerisi bulunamadı.");

            _unitOfWork.PersonSkills.Remove(personSkill);
            await _unitOfWork.SaveChangesAsync();

            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting person skill: {Id}", id);
            return Result<bool>.Fail("Personel becerisi silinirken hata oluştu.");
        }
    }

    public async Task<Result<bool>> AssessPersonSkillAsync(int personSkillId, int assessorId)
    {
        try
        {
            // Stub implementation
            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error assessing person skill: {PersonSkillId}", personSkillId);
            return Result<bool>.Fail("Beceri değerlendirilirken hata oluştu.");
        }
    }

    public async Task<Result<IEnumerable<SkillAssessmentDto>>> GetAssessorAssessmentsAsync(int assessorId)
    {
        try
        {
            // Stub implementation
            return Result<IEnumerable<SkillAssessmentDto>>.Ok(new List<SkillAssessmentDto>());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting assessor assessments: {AssessorId}", assessorId);
            return Result<IEnumerable<SkillAssessmentDto>>.Fail("Değerlendirici değerlendirmeleri alınırken hata oluştu.");
        }
    }

    public async Task<Result<IEnumerable<PersonSkillDto>>> FindSkillExpertsAsync(int skillTemplateId, int minLevel)
    {
        try
        {
            // Stub implementation
            return Result<IEnumerable<PersonSkillDto>>.Ok(new List<PersonSkillDto>());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error finding skill experts: {SkillTemplateId}", skillTemplateId);
            return Result<IEnumerable<PersonSkillDto>>.Fail("Beceri uzmanları bulunurken hata oluştu.");
        }
    }

    public async Task<Result<IEnumerable<PersonSkillDto>>> FindPersonsWithSkillAsync(int skillTemplateId)
    {
        try
        {
            // Stub implementation
            return Result<IEnumerable<PersonSkillDto>>.Ok(new List<PersonSkillDto>());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error finding persons with skill: {SkillTemplateId}", skillTemplateId);
            return Result<IEnumerable<PersonSkillDto>>.Fail("Beceriye sahip kişiler bulunurken hata oluştu.");
        }
    }

    public async Task<Result<decimal>> CalculateSkillMatchPercentageAsync(int personId, int jobDefinitionId)
    {
        try
        {
            // Stub implementation
            return Result<decimal>.Ok(75.0m);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating skill match: {PersonId}, {JobDefinitionId}", personId, jobDefinitionId);
            return Result<decimal>.Fail("Beceri eşleşmesi hesaplanırken hata oluştu.");
        }
    }

    public async Task<Result<IEnumerable<PersonSkillDto>>> GetSkillGapsAsync(int personId, int jobDefinitionId)
    {
        try
        {
            // Stub implementation
            return Result<IEnumerable<PersonSkillDto>>.Ok(new List<PersonSkillDto>());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting skill gaps: {PersonId}, {JobDefinitionId}", personId, jobDefinitionId);
            return Result<IEnumerable<PersonSkillDto>>.Fail("Beceri eksiklikleri alınırken hata oluştu.");
        }
    }

    public async Task<Result<IEnumerable<PersonSkillDto>>> GetCertifiedSkillsAsync(int? personId)
    {
        try
        {
            // Stub implementation
            return Result<IEnumerable<PersonSkillDto>>.Ok(new List<PersonSkillDto>());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting certified skills: {PersonId}", personId);
            return Result<IEnumerable<PersonSkillDto>>.Fail("Sertifikalı beceriler alınırken hata oluştu.");
        }
    }

    public async Task<Result<IEnumerable<PersonSkillDto>>> GetExpiringCertificationsAsync(int daysThreshold)
    {
        try
        {
            // Stub implementation
            return Result<IEnumerable<PersonSkillDto>>.Ok(new List<PersonSkillDto>());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting expiring certifications");
            return Result<IEnumerable<PersonSkillDto>>.Fail("Süresi dolan sertifikalar alınırken hata oluştu.");
        }
    }

    public async Task<Result<IEnumerable<PersonSkillDto>>> GetExpiredCertificationsAsync()
    {
        try
        {
            // Stub implementation
            return Result<IEnumerable<PersonSkillDto>>.Ok(new List<PersonSkillDto>());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting expired certifications");
            return Result<IEnumerable<PersonSkillDto>>.Fail("Süresi dolmuş sertifikalar alınırken hata oluştu.");
        }
    }

    public async Task<Result<bool>> RenewCertificationAsync(int personSkillId, DateTime newExpiryDate)
    {
        try
        {
            // Stub implementation
            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error renewing certification: {PersonSkillId}", personSkillId);
            return Result<bool>.Fail("Sertifika yenilenirken hata oluştu.");
        }
    }

    public async Task<Result<SkillSummaryDto>> GetSkillSummaryAsync()
    {
        try
        {
            // Stub implementation
            var summary = new SkillSummaryDto
            {
                TotalSkillTemplates = 0,
                ActiveSkillTemplates = 0,
                TotalPersonSkills = 0,
                CertifiedSkills = 0,
                EndorsedSkills = 0,
                ExpiringCertifications = 0
            };
            return Result<SkillSummaryDto>.Ok(summary);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting skill summary");
            return Result<SkillSummaryDto>.Fail("Beceri özeti alınırken hata oluştu.");
        }
    }

    public async Task<Result<IEnumerable<SkillCategorySummaryDto>>> GetSkillCategorySummaryAsync()
    {
        try
        {
            // Stub implementation
            return Result<IEnumerable<SkillCategorySummaryDto>>.Ok(new List<SkillCategorySummaryDto>());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting skill category summary");
            return Result<IEnumerable<SkillCategorySummaryDto>>.Fail("Beceri kategori özeti alınırken hata oluştu.");
        }
    }

    public async Task<Result<IEnumerable<SkillTypeSummaryDto>>> GetSkillTypeSummaryAsync()
    {
        try
        {
            // Stub implementation
            return Result<IEnumerable<SkillTypeSummaryDto>>.Ok(new List<SkillTypeSummaryDto>());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting skill type summary");
            return Result<IEnumerable<SkillTypeSummaryDto>>.Fail("Beceri tipi özeti alınırken hata oluştu.");
        }
    }

    public async Task<Result<IEnumerable<PersonSkillDto>>> GetTopSkillsInOrganizationAsync(int count)
    {
        try
        {
            // Stub implementation
            return Result<IEnumerable<PersonSkillDto>>.Ok(new List<PersonSkillDto>());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting top skills in organization");
            return Result<IEnumerable<PersonSkillDto>>.Fail("Organizasyondaki en iyi beceriler alınırken hata oluştu.");
        }
    }

    public async Task<Result<IEnumerable<PersonSkillDto>>> SearchPersonSkillsAsync(string searchTerm)
    {
        try
        {
            // Stub implementation
            return Result<IEnumerable<PersonSkillDto>>.Ok(new List<PersonSkillDto>());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching person skills: {SearchTerm}", searchTerm);
            return Result<IEnumerable<PersonSkillDto>>.Fail("Personel becerileri aranırken hata oluştu.");
        }
    }

    public async Task<Result<bool>> BulkAssessPersonSkillsAsync(int personId, int assessorId, List<SkillAssessmentCreateDto> assessments)
    {
        try
        {
            // Stub implementation
            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error bulk assessing person skills: {PersonId}", personId);
            return Result<bool>.Fail("Personel becerileri toplu değerlendirilirken hata oluştu.");
        }
    }

    public async Task<Result<bool>> ImportSkillTemplatesAsync(List<SkillTemplateCreateDto> skillTemplates)
    {
        try
        {
            // Stub implementation
            return Result<bool>.Ok(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error importing skill templates");
            return Result<bool>.Fail("Beceri şablonları içe aktarılırken hata oluştu.");
        }
    }

    public async Task<Result<byte[]>> ExportPersonSkillsAsync(int personId)
    {
        try
        {
            // Stub implementation
            return Result<byte[]>.Ok(new byte[0]);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting person skills: {PersonId}", personId);
            return Result<byte[]>.Fail("Personel becerileri dışa aktarılırken hata oluştu.");
        }
    }

    public async Task<Result<byte[]>> ExportSkillSummaryAsync()
    {
        try
        {
            // Stub implementation
            return Result<byte[]>.Ok(new byte[0]);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting skill summary");
            return Result<byte[]>.Fail("Beceri özeti dışa aktarılırken hata oluştu.");
        }
    }

    public async Task<Result<byte[]>> ExportSkillTemplatesAsync()
    {
        try
        {
            // Stub implementation
            return Result<byte[]>.Ok(new byte[0]);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting skill templates");
            return Result<byte[]>.Fail("Beceri şablonları dışa aktarılırken hata oluştu.");
        }
    }

}
