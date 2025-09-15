using DAL.Context;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class JobDefinitionRepository : Repository<JobDefinition>, IJobDefinitionRepository
{
    public JobDefinitionRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<JobDefinition>> GetByPositionIdAsync(int positionId)
    {
        return await _context.JobDefinitions
            .Include(x => x.Position)
                .ThenInclude(x => x.Department)
            .Include(x => x.ApprovedBy)
            .Where(x => x.PositionId == positionId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<JobDefinition>> GetApprovedAsync()
    {
        return await _context.JobDefinitions
            .Include(x => x.Position)
                .ThenInclude(x => x.Department)
            .Include(x => x.ApprovedBy)
            .Where(x => x.IsApproved)
            .OrderByDescending(x => x.ApprovedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<JobDefinition>> GetPendingApprovalAsync()
    {
        return await _context.JobDefinitions
            .Include(x => x.Position)
                .ThenInclude(x => x.Department)
            .Where(x => !x.IsApproved)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    public async Task<JobDefinition?> GetByPositionIdAndVersionAsync(int positionId, string version)
    {
        return await _context.JobDefinitions
            .Include(x => x.Position)
                .ThenInclude(x => x.Department)
            .Include(x => x.RequiredQualifications)
            .Include(x => x.ApprovedBy)
            .FirstOrDefaultAsync(x => x.PositionId == positionId && x.Version == version);
    }

    public async Task<IEnumerable<JobDefinition>> GetByDepartmentIdAsync(int departmentId)
    {
        return await _context.JobDefinitions
            .Include(x => x.Position)
                .ThenInclude(x => x.Department)
            .Include(x => x.ApprovedBy)
            .Where(x => x.Position.DepartmentId == departmentId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    public async Task<string> GetNextVersionAsync(int positionId)
    {
        var latestVersion = await _context.JobDefinitions
            .Where(x => x.PositionId == positionId)
            .OrderByDescending(x => x.Version)
            .Select(x => x.Version)
            .FirstOrDefaultAsync();

        if (string.IsNullOrEmpty(latestVersion))
            return "1.0";

        // Simple version increment logic (e.g., 1.0 -> 1.1 -> 1.2 -> 2.0)
        var parts = latestVersion.Split('.');
        if (parts.Length == 2 && int.TryParse(parts[0], out var major) && int.TryParse(parts[1], out var minor))
        {
            minor++;
            if (minor >= 10)
            {
                major++;
                minor = 0;
            }
            return $"{major}.{minor}";
        }

        return "1.0";
    }

    public async Task<IEnumerable<JobDefinition>> GetWithRequiredQualificationsAsync()
    {
        return await _context.JobDefinitions
            .Include(x => x.Position)
                .ThenInclude(x => x.Department)
            .Include(x => x.RequiredQualifications)
            .Include(x => x.ApprovedBy)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<JobDefinition>> GetWithMatchingResultsAsync()
    {
        return await _context.JobDefinitions
            .Include(x => x.Position)
                .ThenInclude(x => x.Department)
            .Include(x => x.MatchingResults)
                .ThenInclude(x => x.Person)
            .Include(x => x.ApprovedBy)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    public async Task<JobDefinition?> GetWithDetailsAsync(int id)
    {
        return await _context.JobDefinitions
            .Include(x => x.Position)
                .ThenInclude(x => x.Department)
            .Include(x => x.RequiredQualifications)
            .Include(x => x.MatchingResults)
                .ThenInclude(x => x.Person)
            .Include(x => x.ApprovedBy)
            .Include(x => x.PreviousVersion)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<bool> HasApprovedVersionAsync(int positionId)
    {
        return await _context.JobDefinitions
            .AnyAsync(x => x.PositionId == positionId && x.IsApproved);
    }

    public async Task<IEnumerable<JobDefinition>> SearchAsync(string searchTerm)
    {
        return await _context.JobDefinitions
            .Include(x => x.Position)
                .ThenInclude(x => x.Department)
            .Include(x => x.ApprovedBy)
            .Where(x => x.Title.Contains(searchTerm) ||
                       x.DetailedDescription.Contains(searchTerm) ||
                       x.Position.Name.Contains(searchTerm) ||
                       x.Position.Department.Name.Contains(searchTerm))
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }
}

public class JobDefinitionQualificationRepository : Repository<JobDefinitionQualification>, IJobDefinitionQualificationRepository
{
    public JobDefinitionQualificationRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<JobDefinitionQualification>> GetByJobDefinitionIdAsync(int jobDefinitionId)
    {
        return await _context.JobDefinitionQualifications
            .Include(x => x.JobDefinition)
            .Where(x => x.JobDefinitionId == jobDefinitionId)
            .OrderBy(x => x.Category)
            .ThenBy(x => x.QualificationName)
            .ToListAsync();
    }

    public async Task<IEnumerable<JobDefinitionQualification>> GetByCategoryAsync(string category)
    {
        return await _context.JobDefinitionQualifications
            .Include(x => x.JobDefinition)
            .Where(x => x.Category == category)
            .OrderBy(x => x.QualificationName)
            .ToListAsync();
    }

    public async Task<IEnumerable<JobDefinitionQualification>> GetByImportanceAsync(QualificationImportance importance)
    {
        return await _context.JobDefinitionQualifications
            .Include(x => x.JobDefinition)
            .Where(x => x.Importance == importance)
            .OrderBy(x => x.Category)
            .ThenBy(x => x.QualificationName)
            .ToListAsync();
    }

    public async Task DeleteByJobDefinitionIdAsync(int jobDefinitionId)
    {
        var qualifications = await _context.JobDefinitionQualifications
            .Where(x => x.JobDefinitionId == jobDefinitionId)
            .ToListAsync();

        _context.JobDefinitionQualifications.RemoveRange(qualifications);
    }
}

public class QualificationMatchingResultRepository : Repository<QualificationMatchingResult>, IQualificationMatchingResultRepository
{
    public QualificationMatchingResultRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<QualificationMatchingResult>> GetByJobDefinitionIdAsync(int jobDefinitionId)
    {
        return await _context.QualificationMatchingResults
            .Include(x => x.JobDefinition)
            .Include(x => x.Person)
            .Include(x => x.ReviewedBy)
            .Where(x => x.JobDefinitionId == jobDefinitionId)
            .OrderByDescending(x => x.OverallMatchPercentage)
            .ToListAsync();
    }

    public async Task<IEnumerable<QualificationMatchingResult>> GetByPersonIdAsync(int personId)
    {
        return await _context.QualificationMatchingResults
            .Include(x => x.JobDefinition)
                .ThenInclude(x => x.Position)
            .Include(x => x.Person)
            .Include(x => x.ReviewedBy)
            .Where(x => x.PersonId == personId)
            .OrderByDescending(x => x.OverallMatchPercentage)
            .ToListAsync();
    }

    public async Task<QualificationMatchingResult?> GetByJobDefinitionAndPersonAsync(int jobDefinitionId, int personId)
    {
        return await _context.QualificationMatchingResults
            .Include(x => x.JobDefinition)
            .Include(x => x.Person)
            .Include(x => x.ReviewedBy)
            .FirstOrDefaultAsync(x => x.JobDefinitionId == jobDefinitionId && x.PersonId == personId);
    }

    public async Task<IEnumerable<QualificationMatchingResult>> GetHighMatchesAsync(decimal minPercentage = 80)
    {
        return await _context.QualificationMatchingResults
            .Include(x => x.JobDefinition)
                .ThenInclude(x => x.Position)
            .Include(x => x.Person)
            .Where(x => x.OverallMatchPercentage >= minPercentage)
            .OrderByDescending(x => x.OverallMatchPercentage)
            .ToListAsync();
    }

    public async Task<IEnumerable<QualificationMatchingResult>> GetByStatusAsync(MatchingStatus status)
    {
        return await _context.QualificationMatchingResults
            .Include(x => x.JobDefinition)
            .Include(x => x.Person)
            .Include(x => x.ReviewedBy)
            .Where(x => x.Status == status)
            .OrderByDescending(x => x.OverallMatchPercentage)
            .ToListAsync();
    }

    public async Task<IEnumerable<QualificationMatchingResult>> GetPendingReviewAsync()
    {
        return await _context.QualificationMatchingResults
            .Include(x => x.JobDefinition)
            .Include(x => x.Person)
            .Where(x => x.Status == MatchingStatus.Pending)
            .OrderByDescending(x => x.OverallMatchPercentage)
            .ToListAsync();
    }

    public async Task<IEnumerable<QualificationMatchingResult>> GetByMatchPercentageRangeAsync(decimal minPercentage, decimal maxPercentage)
    {
        return await _context.QualificationMatchingResults
            .Include(x => x.JobDefinition)
            .Include(x => x.Person)
            .Where(x => x.OverallMatchPercentage >= minPercentage && x.OverallMatchPercentage <= maxPercentage)
            .OrderByDescending(x => x.OverallMatchPercentage)
            .ToListAsync();
    }

    public async Task<bool> ExistsAsync(int jobDefinitionId, int personId)
    {
        return await _context.QualificationMatchingResults
            .AnyAsync(x => x.JobDefinitionId == jobDefinitionId && x.PersonId == personId);
    }

    public async Task DeleteByJobDefinitionIdAsync(int jobDefinitionId)
    {
        var results = await _context.QualificationMatchingResults
            .Where(x => x.JobDefinitionId == jobDefinitionId)
            .ToListAsync();

        _context.QualificationMatchingResults.RemoveRange(results);
    }

    public async Task DeleteByPersonIdAsync(int personId)
    {
        var results = await _context.QualificationMatchingResults
            .Where(x => x.PersonId == personId)
            .ToListAsync();

        _context.QualificationMatchingResults.RemoveRange(results);
    }
}
