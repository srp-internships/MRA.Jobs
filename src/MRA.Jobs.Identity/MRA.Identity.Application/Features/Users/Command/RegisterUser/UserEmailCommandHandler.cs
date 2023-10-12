using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MRA.Identity.Application.Common.Interfaces.Services;
using MRA.Identity.Application.Contract.User.Commands;
using MRA.Identity.Domain.Entities;

namespace MRA.Identity.Application.Features.Users.Command.RegisterUser;
public class UserEmailCommandHandler : IRequestHandler<UserEmailCommand,Guid>
{

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserHttpContextAccessor _user;
    private readonly IEmailVerification _emailVerification;

    public UserEmailCommandHandler(UserManager<ApplicationUser> userManager, IUserHttpContextAccessor user, IEmailVerification emailVerification)
    {
        _userManager = userManager;
        _user = user;
        _emailVerification = emailVerification;
    }

    public async Task<Guid> Handle(UserEmailCommand request, CancellationToken cancellationToken)
    {
        var userId = _user.GetUserId();
        var user = await _userManager.Users.SingleOrDefaultAsync(s => s.Id == userId);
        await _emailVerification.SendVerificationEmailAsync(user);
        return Guid.Empty;
    }
}
