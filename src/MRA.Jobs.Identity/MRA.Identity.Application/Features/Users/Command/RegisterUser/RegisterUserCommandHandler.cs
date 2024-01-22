using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MRA.Configurations.Common.Constants;
using MRA.Identity.Application.Common.Exceptions;
using MRA.Identity.Application.Common.Interfaces.DbContexts;
using MRA.Identity.Application.Common.Interfaces.Services;
using MRA.Identity.Application.Contract.User.Commands.RegisterUser;
using MRA.Identity.Domain.Entities;
using Newtonsoft.Json;

namespace MRA.Identity.Application.Features.Users.Command.RegisterUser;

public class RegisterUserCommandHandler(
    UserManager<ApplicationUser> userManager,
    IApplicationDbContext context,
    IEmailVerification emailVerification,
    ISmsCodeChecker codeChecker)
    : IRequestHandler<RegisterUserCommand, Guid>
{
    public async Task<Guid> Handle(RegisterUserCommand request,
        CancellationToken cancellationToken)
    {
        var exitingUser = await context.Users.FirstOrDefaultAsync(
            u => u.PhoneNumber == request.PhoneNumber || u.Email == request.Email,
            cancellationToken: cancellationToken);
        if (exitingUser != null)
        {
            if (exitingUser.Email == request.Email && exitingUser.PhoneNumber == request.PhoneNumber)
            {
                throw new DuplicateWaitObjectException(
                    $"Email {request.Email} and Phone Number {request.PhoneNumber} are not available!");
            }

            if (exitingUser.PhoneNumber == request.PhoneNumber)
            {
                throw new DuplicateWaitObjectException($"Phone Number {request.PhoneNumber} is not available!");
            }

            if (exitingUser.Email == request.Email)
            {
                throw new DuplicateWaitObjectException($"Email {request.Email} is not available!");
            }
        }

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
            DateOfBirth = new DateTime(2000, 1, 1)
        };
        bool phoneVerified = codeChecker.VerifyPhone(request.VerificationCode, request.PhoneNumber);
        if (phoneVerified) user.PhoneNumberConfirmed = true;
        else throw new ValidationException("Phone number is not verified");

        IdentityResult result = await userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            throw new UnauthorizedAccessException(result.Errors.First().Description);
        }

        await emailVerification.SendVerificationEmailAsync(user);

        var role = await context.Roles.FirstOrDefaultAsync(s => s.Name == "Applicant",
            cancellationToken: cancellationToken);

        var userRole =
            new ApplicationUserRole { UserId = user.Id, RoleId = role.Id, Slug = $"{user.UserName}-Applicant" };
        await context.UserRoles.AddAsync(userRole, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        await CreateClaimAsync(user.UserName, user.Id, user.Email, cancellationToken);
        return user.Id;
    }


    private async Task CreateClaimAsync(string username, Guid id, string email,
        CancellationToken cancellationToken = default)
    {
        var userClaims = new[]
        {
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
            }
        };
        await context.UserClaims.AddRangeAsync(userClaims, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
}