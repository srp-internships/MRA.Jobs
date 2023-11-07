using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MRA.Identity.Application.Common.Exceptions;
using MRA.Identity.Application.Contract.User.Queries.GetUserNameByPhoneNymber;
using MRA.Identity.Domain.Entities;

namespace MRA.Identity.Application.Features.Users.Query;
public class IsAvailableUserPhoneNumberQueryHandler : IRequestHandler<IsAvailableUserPhoneNumberQuery, bool>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public IsAvailableUserPhoneNumberQueryHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }
    public async Task<bool> Handle(IsAvailableUserPhoneNumberQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber);
        if (user == null)
            return true;

        return false;
    }
}
