using System.Text.RegularExpressions;
using MRA.Jobs.Application.Contracts.Identity.Responces;
using MRA.Jobs.Application.Contracts.MyProfile.Commands;
using MRA.Jobs.Application.Contracts.MyProfile.Responses;

namespace MRA.Jobs.Application.Features.Me.Commands;

public class UpdateMyProfileCommandValidator : AbstractValidator<UpdateMyProfileCommand>
{
    private readonly string pattern = "^[a-zA-Zа-яА-Я]+$";

    public UpdateMyProfileCommandValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().Must(s => Regex.IsMatch(s, pattern)).WithMessage("Unacceptable symbols");
        RuleFor(x => x.LastName).NotEmpty().Must(s => Regex.IsMatch(s, pattern)).WithMessage("Unacceptable symbols");
        RuleFor(x => x.Patronymic).Must(s => string.IsNullOrEmpty(s) || Regex.IsMatch(s, pattern))
            .WithMessage("Unacceptable symbols");
        RuleFor(w => w.DateOfBirth).NotEmpty();
        RuleFor(w => w.Gender).Must(g => Enum.IsDefined(g));
    }
}

public class UpdateMyProfileCommandHandler : IRequestHandler<UpdateMyProfileCommand, MyProfileResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    // private readonly IIdentityService _identityService;

    public UpdateMyProfileCommandHandler(ICurrentUserService currentUser,
        IApplicationDbContext context)
    {
        // _identityService = identityService;
        _currentUser = currentUser;
        _context = context;
    }

    public async Task<MyProfileResponse> Handle(UpdateMyProfileCommand command, CancellationToken cancellationToken)
    {
        Guid? userId = _currentUser.GetId();
        if (userId is null)
        {
            throw new UnauthorizedAccessException();
        }

        if (cancellationToken.IsCancellationRequested)
        {
            throw new TaskCanceledException(nameof(cancellationToken));
        }

        User domainUser = await _context.DomainUsers.FindAsync(userId, cancellationToken);
        if (domainUser is null)
        {
            throw new NotFoundException(nameof(User), userId);
        }

        domainUser.DateOfBirth = command.DateOfBirth;
        domainUser.FirstName = command.FirstName;
        domainUser.LastName = command.LastName;
        domainUser.Patronymic = command.Patronymic;
        domainUser.Avatar = command.Avatar;
        domainUser.Gender = command.Gender;
        await _context.SaveChangesAsync(cancellationToken);


        // UserIdentityResponse identityProfile =
            // await _identityService.GetUserIdentityAsync(userId.Value, cancellationToken);

        MyProfileResponse response = new MyProfileResponse
        {
            Avatar = domainUser.Avatar,
            DateOfBirth = domainUser.DateOfBirth,
            Email = domainUser.Email,
            FirstName = domainUser.FirstName,
            Gender = domainUser.Gender,
            Identity = new MyIdentityResponse
            {
                // IsActive = identityProfile.IsActive,
                // Permissions = identityProfile.Permissions,
                // Roles = identityProfile.Roles,
                // TwoFactorEnabled = identityProfile.TwoFactorEnabled
            },
            LastName = domainUser.LastName,
            Patronymic = domainUser.Patronymic,
            PhoneNumber = domainUser.PhoneNumber
        };

        return response;
    }
}