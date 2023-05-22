using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Common.Interfaces;
using MRA.Jobs.Application.Common.Security;
using MRA.Jobs.Application.Contracts.Users.Commands;
using MRA.Jobs.Application.Contracts.Users.Responses;

namespace MRA.Jobs.Application.Features.Me.Commands;

public class UpdateMyProfileCommandValidator : AbstractValidator<UpdateMyProfileCommand>
{
    private readonly string pattern = "^[a-zA-Zа-яА-Я]+$";
    public UpdateMyProfileCommandValidator()
    {

        RuleFor(x => x.FirstName).NotEmpty().Must(s => Regex.IsMatch(s, pattern)).WithMessage("Unacceptable symbols");
        RuleFor(x => x.LastName).NotEmpty().Must(s => Regex.IsMatch(s, pattern)).WithMessage("Unacceptable symbols");
        RuleFor(x => x.Patronymic).Must(s => string.IsNullOrEmpty(s) || Regex.IsMatch(s, pattern)).WithMessage("Unacceptable symbols");
        RuleFor(w => w.DateOfBirth).NotEmpty();
        RuleFor(w => w.Gender).Must(g => Enum.IsDefined(g));
    }
}

public class UpdateMyProfileCommandHandler : IRequestHandler<UpdateMyProfileCommand, MyProfileResponse>
{
    private readonly IIdentityService _identityService;
    private readonly ICurrentUserService _currentUser;
    private readonly IApplicationDbContext _context;

    public UpdateMyProfileCommandHandler(IIdentityService identityService, ICurrentUserService currentUser, IApplicationDbContext context)
    {
        _identityService = identityService;
        _currentUser = currentUser;
        _context = context;
    }

    public async Task<MyProfileResponse> Handle(UpdateMyProfileCommand command, CancellationToken cancellationToken)
    {
        var userId = _currentUser.GetId();
        if (userId is null)
            throw new UnauthorizedAccessException();

        if (cancellationToken.IsCancellationRequested)
            throw new TaskCanceledException(nameof(cancellationToken));

        var domainUser = await _context.DomainUsers.FindAsync(userId, cancellationToken);
        if (domainUser is null)
            throw new NotFoundException(nameof(User), userId);

        domainUser.DateOfBirth = command.DateOfBirth;
        domainUser.FirstName = command.FirstName;
        domainUser.LastName = command.LastName;
        domainUser.Patronymic = command.Patronymic;
        domainUser.Avatar = command.Avatar;
        domainUser.Gender = command.Gender;
        await _context.SaveChangesAsync(cancellationToken);


        var identityProfile = await _identityService.GetUserIdentityAsync(userId.Value, cancellationToken);

        var response = new MyProfileResponse
        {
            Avatar = domainUser.Avatar,
            DateOfBirth = domainUser.DateOfBirth,
            Email = domainUser.Email,
            FirstName = domainUser.FirstName,
            Gender = domainUser.Gender,
            Identity = new()
            {
                IsActive = identityProfile.IsActive,
                Permissions = identityProfile.Permissions,
                Roles = identityProfile.Roles,
                TwoFactorEnabled = identityProfile.TwoFactorEnabled,
            },
            LastName = domainUser.LastName,
            Patronymic = domainUser.Patronymic,
            PhoneNumber = domainUser.PhoneNumber
        };

        return response;
    }
}
