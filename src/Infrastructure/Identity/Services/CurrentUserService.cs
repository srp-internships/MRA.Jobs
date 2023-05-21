namespace MRA.Jobs.Infrastructure.Identity.Services;

public class CurrentUserService : ICurrentUserService
{
    internal Guid Id { get; set; }
    internal string UserName { get; set; }
    internal string Email { get; set; }
    internal string[] Roles { get; set; }

    public CurrentUserService() { }

    public async Task<Guid> GetIdAsync()
    {
        return await Task.FromResult(Id);
    }

    public async Task<string> GetUserNameAsync()
    {
        return await Task.FromResult(UserName);
    }

    public Task<bool> HasPermissionAsync(Guid userId, string role)
    {
        return Task.FromResult(true);
    }

    public Guid GetId()
    {
        return Id;
    }

    public string GetUserName()
    {
        return UserName;
    }

    public bool HasPermission(Guid userId, string permission)
    {
        return true;
    }
}
