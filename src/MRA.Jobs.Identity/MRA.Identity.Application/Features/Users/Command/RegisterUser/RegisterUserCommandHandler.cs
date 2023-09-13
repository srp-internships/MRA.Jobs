using MediatR;
using Microsoft.AspNetCore.Identity;
using MRA.Identity.Application.Common.Interfaces.DbContexts;
using MRA.Identity.Application.Common.Interfaces.Services;
using MRA.Identity.Application.Contract;
using MRA.Identity.Application.Contract.User.Commands;
using MRA.Identity.Domain.Entities;
using Mra.Shared.Common.Constants;

namespace MRA.Identity.Application.Features.Users.Command.RegisterUser;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, ApplicationResponse<Guid>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IApplicationDbContext _context;
    private readonly IEmailVerification _emailVerification;
    public RegisterUserCommandHandler(UserManager<ApplicationUser> userManager, IApplicationDbContext context, IEmailVerification emailVerification)
    {
        _userManager = userManager;
        _context = context;
        _emailVerification = emailVerification;
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
                EmailConfirmed = false,
                PhoneNumber = request.PhoneNumber
            };
            IdentityResult result = await _userManager.CreateAsync(user, request.Password);
            
            if (!result.Succeeded)
            {
                return new ApplicationResponseBuilder<Guid>().SetErrorMessage(result.Errors.First().Description).Success(false).Build();
            }

            if (result.Succeeded)
            {
                await _emailVerification.SendVerificationEmailAsync(user);
            }

            var idClaim = new ApplicationUserClaim
            {
                ClaimType = ClaimTypes.Id,
                ClaimValue = user.Id.ToString(),
                UserId = user.Id,
                Slug = user.UserName + "-id"
            };
            await _context.UserClaims.AddAsync(idClaim, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return new ApplicationResponseBuilder<Guid>().SetResponse(user.Id).Build();
        }
        catch (Exception e)
        {
            return new ApplicationResponseBuilder<Guid>().SetException(e).Success(false).Build();
        }
    }
}