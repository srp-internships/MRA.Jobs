namespace MRA.Jobs.Application.Common.Security;

public interface ICurrentUserService
{
    Guid? GetId();
    Task<Guid?> GetIdAsync(CancellationToken cancellationToken);

    Task<string> GetUserNameAsync(CancellationToken cancellationToken);
    string GetUserName();

    Task<bool> HasPermissionAsync(string role, CancellationToken cancellationToken);
    bool HasPermission(string permission);
}