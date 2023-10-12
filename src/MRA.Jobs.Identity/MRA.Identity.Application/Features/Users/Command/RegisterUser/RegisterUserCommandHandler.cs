using MediatR;
using Microsoft.AspNetCore.Identity;
using MRA.Identity.Application.Common.Interfaces.DbContexts;
using MRA.Identity.Application.Common.Interfaces.Services;
using MRA.Identity.Application.Contract;
using MRA.Identity.Domain.Entities;
using Mra.Shared.Common.Constants;
using MRA.Identity.Application.Contract.User.Commands.RegisterUser;
using MRA.Identity.Application.Common.Exceptions;

namespace MRA.Identity.Application.Features.Users.Command.RegisterUser;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Guid>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IApplicationDbContext _context;
    private readonly IEmailVerification _emailVerification;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public RegisterUserCommandHandler(UserManager<ApplicationUser> userManager, IApplicationDbContext context,
        IEmailVerification emailVerification, RoleManager<ApplicationRole> roleManager)
    {
        _userManager = userManager;
        _context = context;
        _emailVerification = emailVerification;
        _roleManager = roleManager;
    }

    public async Task<Guid> Handle(RegisterUserCommand request,
        CancellationToken cancellationToken)
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
            throw new ValidationException(result.Errors.First().Description);
        }

        await _emailVerification.SendVerificationEmailAsync(user);

        if (!await _roleManager.RoleExistsAsync(request.Role))
        {
            var role = new ApplicationRole
            {
                Id = Guid.NewGuid(),
                Name = request.Role,
                NormalizedName = _roleManager.NormalizeKey(request.Role),
                Slug = $"{request.Username}-{request.Role}",
            };
            var roleResult = await _roleManager.CreateAsync(role);
            if (!roleResult.Succeeded)
            {
                throw new ValidationException(roleResult.Errors.First().Description);

            }
        }

        var userRoleResult = await _userManager.AddToRoleAsync(user, request.Role);
        if (!userRoleResult.Succeeded)
        {
            
            throw new ValidationException(userRoleResult.Errors.First().Description);
        }
        await CreateClaimAsync(request.Role, user.UserName, user.Id, user.Email, user.PhoneNumber, request.Application,
            cancellationToken);
        return user.Id;
    }


    private async Task CreateClaimAsync(string role, string username, Guid id, string email, string phone,
        string application, CancellationToken cancellationToken = default)
    {
        var userClaims = new[]
        {
            new ApplicationUserClaim
            {
                UserId = id, ClaimType = ClaimTypes.Role, ClaimValue = role, Slug = $"{username}-role"
            },
            new ApplicationUserClaim
            {
                UserId = id, ClaimType = ClaimTypes.Id, ClaimValue = id.ToString(), Slug = $"{username}-id"
            },
            new ApplicationUserClaim
            {
                UserId = id,
                ClaimType = ClaimTypes.Username,
                ClaimValue = username,
                Slug = $"{username}-username"
            },
            new ApplicationUserClaim
            {
                UserId = id, ClaimType = ClaimTypes.Email, ClaimValue = email, Slug = $"{username}-email"
            },
            new ApplicationUserClaim
            {
                UserId = id,
                ClaimType = ClaimTypes.Application,
                ClaimValue = application,
                Slug = $"{username}-application"
            }
        };
        await _context.UserClaims.AddRangeAsync(userClaims, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}