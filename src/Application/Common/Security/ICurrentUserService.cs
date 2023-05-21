namespace MRA.Jobs.Application.Common.Security;

public interface ICurrentUserService
{
    Guid GetId();
    Task<Guid> GetIdAsync();
    Task<string> GetUserNameAsync();
    string GetUserName();
    Task<bool> HasPermissionAsync(Guid userId, string role);
    bool HasPermission(Guid userId, string permission);
}
