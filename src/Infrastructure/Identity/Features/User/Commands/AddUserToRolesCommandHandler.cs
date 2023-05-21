using Microsoft.AspNetCore.Identity;
using MRA.Jobs.Infrastructure.Shared.Users.Commands;

namespace MRA.Jobs.Infrastructure.Identity.Features.User.Commands;

public class AddUserToRolesCommandHandler : IRequestHandler<AddUserToRolesCommand, Unit>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public AddUserToRolesCommandHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Unit> Handle(AddUserToRolesCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.Id.ToString());
        if (user == null)
            throw new NotFoundException(nameof(ApplicationUser), request.Id);

        var result = await _userManager.AddToRolesAsync(user, request.Roles);
        if (!result.Succeeded)
            throw new BusinessLogicException(string.Join('\n', result.Errors.Select(r => r.Description)));

        return Unit.Value;
    }
}
