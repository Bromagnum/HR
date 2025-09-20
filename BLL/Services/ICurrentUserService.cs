namespace BLL.Services;

public interface ICurrentUserService
{
    int? UserId { get; }
    string? UserName { get; }
    string? Email { get; }
    int? PersonId { get; }
    int? DepartmentId { get; }
    int? BranchId { get; }
    IEnumerable<string> Roles { get; }
    IEnumerable<string> Claims { get; }
    bool IsInRole(string role);
    bool HasClaim(string claimType, string claimValue);
    string? GetClaimValue(string claimType);
    int GetCurrentUserId();
}
