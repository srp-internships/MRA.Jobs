using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using MRA.Identity.Application.Common.Exceptions;
using MRA.Identity.Application.Common.Interfaces.Services;
using MRA.Identity.Application.Contract.User.Commands.ChangePassword;
using MRA.Identity.Domain.Entities;
namespace MRA.Identity.Application.Features.Users.Command.UserPassword;
public class ChangePasswordUserCommandHandler : IRequestHandler<ChangePasswordUserCommand, bool>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserHttpContextAccessor _userHttpContextAccessor;

    public ChangePasswordUserCommandHandler(UserManager<ApplicationUser> userManager, IUserHttpContextAccessor userHttpContextAccessor)
    {
        _userManager = userManager;
        _userHttpContextAccessor = userHttpContextAccessor;
    }

    public async Task<bool> Handle(ChangePasswordUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(_userHttpContextAccessor.GetUserName());
        if (user == null)
            throw new NotFoundException("User not found");

        bool success = await _userManager.CheckPasswordAsync(user, request.OldPassword);
        if (!success)
            throw new Exception("Incorrect old password");

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        var result = await _userManager.ResetPasswordAsync(user, token, request.NewPassword);

        if (!result.Succeeded)
            return false;

        return true;
    }
}
