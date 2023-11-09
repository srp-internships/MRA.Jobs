using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MRA.Identity.Application.Common.Interfaces.DbContexts;
using MRA.Identity.Application.Common.Interfaces.Services;
using MRA.Identity.Domain.Entities;
using MRA.Configurations.Common.Constants;
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

        var userDb = await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber);
        if (userDb != null)
            throw new DuplicateWaitObjectException($"Phone number {request.PhoneNumber} is not available!");

        ApplicationUser user = new()
        {
            Id = Guid.NewGuid(),
            UserName = request.Username,
            NormalizedUserName = request.Username.ToLower(),
            Email = request.Email,
            NormalizedEmail = request.Email.ToLower(),
            EmailConfirmed = false,
            PhoneNumber = request.PhoneNumber,
            FirstName = request.FirstName,
            LastName = request.LastName,
            DateOfBirth = new DateTime(2000, 1, 1),
            PhoneNumberConfirmed = request.PhoneNumberConfirmed
        };
        IdentityResult result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            throw new UnauthorizedAccessException(result.Errors.First().Description);
        }

        await _emailVerification.SendVerificationEmailAsync(user);

        var role = await _context.Roles.FirstOrDefaultAsync(s => s.NormalizedName != null && s.NormalizedName.Contains(request.Role.ToUpper()), cancellationToken: cancellationToken);
        if (role == null)
        {
            role = new ApplicationRole
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

        var userRole = new ApplicationUserRole { UserId = user.Id, RoleId = role.Id, Slug = $"{user.UserName}-role" };
        await _context.UserRoles.AddAsync(userRole, cancellationToken);
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