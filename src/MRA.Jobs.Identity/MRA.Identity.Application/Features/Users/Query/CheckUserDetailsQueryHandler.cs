using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MRA.Identity.Application.Contract.User.Queries.CheckUserDetails;
using MRA.Identity.Application.Contract.User.Responses;
using MRA.Identity.Domain.Entities;

namespace MRA.Identity.Application.Features.Users.Query;
public class CheckUserDetailsQueryHandler(UserManager<ApplicationUser> userManager) : IRequestHandler<CheckUserDetailsQuery, UserDetailsResponse>
{
    private readonly UserManager<ApplicationUser> _userManager=userManager;
   
    public async Task<UserDetailsResponse> Handle(CheckUserDetailsQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.UserName);
        var userWithPhone = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber);
        var userWithEmail = await _userManager.FindByEmailAsync(request.Email);

        return new UserDetailsResponse
        {
            IsUserNameTaken = user != null,
            IsPhoneNumberTaken = userWithPhone != null,
            IsEmailTaken = userWithEmail != null
        };
    }
}
