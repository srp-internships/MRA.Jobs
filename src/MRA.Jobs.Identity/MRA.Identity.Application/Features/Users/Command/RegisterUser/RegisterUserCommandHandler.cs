using MediatR;
using Microsoft.AspNetCore.Identity;
using MRA.Identity.Application.Contract;
using MRA.Identity.Application.Contract.User.Commands;
using MRA.Identity.Domain.Entities;
using Newtonsoft.Json;

namespace MRA.Identity.Application.Features.Users.Command.RegisterUser;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, ApplicationResponse<Guid>>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public RegisterUserCommandHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<ApplicationResponse<Guid>> Handle(RegisterUserCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            ApplicationUser user = new()
            {
                Id = Guid.NewGuid(),
                UserName = request.Username,
                NormalizedUserName = request.Username.ToLower(),
                Email = request.Email,
                NormalizedEmail = request.Email.ToLower(),
                EmailConfirmed = false
            };
            IdentityResult result = await _userManager.CreateAsync(user, request.Password);

            return result.Succeeded
                ? new ApplicationResponseBuilder<Guid>().SetResponse(user.Id).Build()
                : new ApplicationResponseBuilder<Guid>().SetErrorMessage(result.Errors.First().Description).Success(false).Build();
        }
        catch (Exception e)
        {
            return new ApplicationResponseBuilder<Guid>().SetException(e).Success(false).Build();
        }
    }
}