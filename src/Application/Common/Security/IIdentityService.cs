namespace MRA.Jobs.Application.Common.Security;

public interface IIdentityService
{
    Task<bool> HasPermissionAsync(Guid userId, string role);

    Task<(AuthResult Result, Guid UserId)> CreateUserAsync(Guid userName, string password);

    Task<AuthResult> DeleteUserAsync(Guid userId);
}
