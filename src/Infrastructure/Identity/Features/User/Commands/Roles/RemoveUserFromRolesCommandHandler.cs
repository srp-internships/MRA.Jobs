using Microsoft.AspNetCore.Identity;
using MRA.Jobs.Infrastructure.Shared.Users.Commands.Roles;

namespace MRA.Jobs.Infrastructure.Identity.Features.User.Commands.Roles;

public class RemoveUserFromRolesCommandHandler : IRequestHandler<RemoveUserFromRolesCommand, Unit>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public RemoveUserFromRolesCommandHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Unit> Handle(RemoveUserFromRolesCommand request, CancellationToken cancellationToken)
    {
        ApplicationUser user = await _userManager.FindByIdAsync(request.Id.ToString());
        if (user == null)
        {
            throw new NotFoundException(nameof(ApplicationUser), request.Id);
        }

        IdentityResult result = await _userManager.RemoveFromRolesAsync(user, request.Roles);
        if (!result.Succeeded)
        {
            throw new ValidationException(string.Join('\n', result.Errors.Select(r => r.Description)));
        }

        return Unit.Value;
    }
}