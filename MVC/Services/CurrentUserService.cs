using System.Security.Claims;
using BLL.Services;

namespace MVC.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int? UserId
    {
        get
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdClaim, out var userId) ? userId : null;
        }
    }

    public string? UserName => _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

    public string? Email => _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;

    public int? PersonId
    {
        get
        {
            var personIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("PersonId")?.Value;
            return int.TryParse(personIdClaim, out var personId) ? personId : null;
        }
    }

    public int? DepartmentId
    {
        get
        {
            var departmentIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("DepartmentId")?.Value;
            return int.TryParse(departmentIdClaim, out var departmentId) ? departmentId : null;
        }
    }

    public int? BranchId
    {
        get
        {
            var branchIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("BranchId")?.Value;
            return int.TryParse(branchIdClaim, out var branchId) ? branchId : null;
        }
    }

    public IEnumerable<string> Roles => 
        _httpContextAccessor.HttpContext?.User?.FindAll(ClaimTypes.Role)?.Select(c => c.Value) ?? 
        Enumerable.Empty<string>();

    public IEnumerable<string> Claims => 
        _httpContextAccessor.HttpContext?.User?.Claims?.Select(c => $"{c.Type}:{c.Value}") ?? 
        Enumerable.Empty<string>();

    public bool IsInRole(string role)
    {
        return _httpContextAccessor.HttpContext?.User?.IsInRole(role) ?? false;
    }

    public bool HasClaim(string claimType, string claimValue)
    {
        return _httpContextAccessor.HttpContext?.User?.HasClaim(claimType, claimValue) ?? false;
    }

    public string? GetClaimValue(string claimType)
    {
        return _httpContextAccessor.HttpContext?.User?.FindFirst(claimType)?.Value;
    }

    public int GetCurrentUserId()
    {
        return UserId ?? 0;
    }
}
