using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MRA.Identity.Application.Common.Interfaces.Services;
using MRA.Identity.Application.Contract;
using MRA.Identity.Application.Contract.User.Commands;
using MRA.Identity.Domain.Entities;

namespace MRA.Identity.Application.Features.Users.Command.RegisterUser;
public class UserEmailCommandHandler : IRequestHandler<UserEmallCommand, ApplicationResponse<Guid>>
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

    public async Task<ApplicationResponse<Guid>> Handle(UserEmallCommand request, CancellationToken cancellationToken)
    {

        try
        {
            var userId=_user.GetUserId();
            var user = await _userManager.Users.SingleOrDefaultAsync(s => s.Id == userId);
            await _emailVerification.SendVerificationEmailAsync(user);

            return new ApplicationResponseBuilder<Guid>().Build();
        }
        catch (Exception e)
        {
            return new ApplicationResponseBuilder<Guid>().Success(false).SetException(e).Build();
        }
    }
}
