using Microsoft.AspNetCore.Authorization;

namespace MRA.Jobs.Infrastructure.Identity.Authorization;

internal class CheckCurrentUserRequirement : IAuthorizationRequirement
{
}

internal class CheckCurrentUserAuthHandler : AuthorizationHandler<CheckCurrentUserRequirement>
{
    private readonly ICurrentUserService _currentUser;

    public CheckCurrentUserAuthHandler(ICurrentUserService currentUser)
    {
        _currentUser = currentUser;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
        CheckCurrentUserRequirement requirement)
    {
        if (_currentUser.GetId() != Guid.Empty)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}