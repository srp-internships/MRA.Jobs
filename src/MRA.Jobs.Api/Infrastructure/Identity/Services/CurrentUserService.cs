namespace MRA.Jobs.Infrastructure.Identity.Services;

public class CurrentUserService : ICurrentUserService
{
    internal Guid? Id { get; set; }
    internal string UserName { get; set; }
    internal string Email { get; set; }
    internal string[] Roles { get; set; }

    public async Task<Guid?> GetIdAsync(CancellationToken cancellationToken)
    {
        return await Task.FromResult(Id);
    }

    public Guid? GetId()
    {
        return Id;
    }

    public async Task<string> GetUserNameAsync(CancellationToken cancellationToken)
    {
        return await Task.FromResult(UserName);
    }

    public string GetUserName()
    {
        return UserName;
    }

    public Task<bool> HasPermissionAsync(string role, CancellationToken cancellationToken)
    {
        return Task.FromResult(true);
    }

    public bool HasPermission(string permission)
    {
        return true;
    }
}