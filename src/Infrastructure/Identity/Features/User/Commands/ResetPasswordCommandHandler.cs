using Microsoft.AspNetCore.Identity;
using MRA.Jobs.Infrastructure.Identity.Utils;
using MRA.Jobs.Infrastructure.Shared.Users.Commands;

namespace MRA.Jobs.Infrastructure.Identity.Features.User.Commands;

public class ResetPasswordCommandHandler : IRequestHandler<ResetUserPasswordCommand, Unit>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ResetPasswordCommandHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Unit> Handle(ResetUserPasswordCommand request, CancellationToken cancellationToken)
    {
        ApplicationUser user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null)
        {
            throw new NotFoundException(nameof(ApplicationUser), request.UserId);
        }

        string token = await _userManager.GeneratePasswordResetTokenAsync(user);
        IdentityResult result = await _userManager.ResetPasswordAsync(user, token, PaswordHelper.RandomPassword());
        if (!result.Succeeded)
        {
            throw new ValidationException(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
        }

        //TODO: Send Email to use with new password

        return Unit.Value;
    }
}