using MRA.Jobs.Application.Contracts.Identity.Responces;
using MRA.Jobs.Application.Contracts.MyProfile.Queries;
using MRA.Jobs.Application.Contracts.MyProfile.Responses;

namespace MRA.Jobs.Application.Features.Me.Queries;

public class GetMyProfileQueryHandler : IRequestHandler<GetMyProfileQuery, MyProfileResponse>
{
    private readonly IApplicationDbContext _applicationDbContext;
    private readonly ICurrentUserService _currentUser;
    private readonly IIdentityService _identityService;

    public GetMyProfileQueryHandler(ICurrentUserService currentUser, IIdentityService identityService,
        IApplicationDbContext applicationDbContext)
    {
        _currentUser = currentUser;
        _identityService = identityService;
        _applicationDbContext = applicationDbContext;
    }

    public async Task<MyProfileResponse> Handle(GetMyProfileQuery command, CancellationToken cancellationToken)
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

        User domainUser = await _applicationDbContext.DomainUsers.FindAsync(new object[] { userId }, cancellationToken);
        if (domainUser is null)
        {
            throw new NotFoundException(nameof(User), userId);
        }

        UserIdentityResponse identityProfile =
            await _identityService.GetUserIdentityAsync(userId.Value, cancellationToken);

        MyProfileResponse response = new MyProfileResponse
        {
            Avatar = domainUser.Avatar,
            DateOfBirth = domainUser.DateOfBirth,
            Email = domainUser.Email,
            FirstName = domainUser.FirstName,
            Gender = domainUser.Gender,
            Identity = new MyIdentityResponse
            {
                IsActive = identityProfile.IsActive,
                Permissions = identityProfile.Permissions,
                Roles = identityProfile.Roles,
                TwoFactorEnabled = identityProfile.TwoFactorEnabled
            },
            LastName = domainUser.LastName,
            Patronymic = domainUser.Patronymic,
            PhoneNumber = domainUser.PhoneNumber
        };

        return response;
    }
}