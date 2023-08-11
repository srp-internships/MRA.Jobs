using MRA.Jobs.Application.Contracts.Identity.Responces;

namespace MRA.Jobs.Application.Common.Security;

public interface IIdentityService
{
    Task<bool> HasPermissionAsync(Guid userId, string permission, CancellationToken cancellationToken);

    Task<UserIdentityResponse> GetUserIdentityAsync(Guid userId, CancellationToken cancellationToken);
}