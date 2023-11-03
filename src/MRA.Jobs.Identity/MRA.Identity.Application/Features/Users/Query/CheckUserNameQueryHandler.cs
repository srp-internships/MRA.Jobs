using MediatR;
using Microsoft.AspNetCore.Identity;
using MRA.Identity.Application.Contract.User.Queries.CheckUserName;
using MRA.Identity.Domain.Entities;

namespace MRA.Identity.Application.Features.Users.Query;
public class CheckUserNameQueryHandler : IRequestHandler<CheckUserNameQuery, bool>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public CheckUserNameQueryHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }
    public async Task<bool> Handle(CheckUserNameQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.UserName);
        if (user == null)
            return false;

        return true;
    }
}
