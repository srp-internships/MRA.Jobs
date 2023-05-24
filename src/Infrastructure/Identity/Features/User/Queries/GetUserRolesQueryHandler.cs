using Microsoft.AspNetCore.Identity;
using MRA.Jobs.Infrastructure.Shared.Users.Queries;

namespace MRA.Jobs.Infrastructure.Identity.Features.User.Queries;

public class GetUserRolesQueryHandler : IRequestHandler<GetUserRolesQuery, IEnumerable<string>>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public GetUserRolesQueryHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IEnumerable<string>> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.Id.ToString());
        if (user == null)
            throw new NotFoundException(nameof(ApplicationUser), request.Id);

        return await _userManager.GetRolesAsync(user);
    }
}
