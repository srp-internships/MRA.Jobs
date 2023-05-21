using Microsoft.AspNetCore.Identity;

namespace MRA.Jobs.Infrastructure.Identity.Features.User.Commands;

public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Unit>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ChangePasswordCommandHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Unit> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.Id.ToString());
        if (user == null)
            throw new EntityNotFoundException(nameof(ApplicationUser), request.Id);

        var result = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
        if (!result.Succeeded)
            throw new BusinessLogicException(string.Join('\n', result.Errors.Select(r => r.Description)));

        return Unit.Value;
    }
}
