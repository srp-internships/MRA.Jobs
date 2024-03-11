using Microsoft.EntityFrameworkCore;
using MRA.Identity.Application.Contract.User.Responses;
using MRA.Jobs.Application.Contracts.Applications.Candidates;

namespace MRA.Jobs.Application.Features.Applications.Candidates;

public class GetCandidatesQueryHandler(
    IUsersService usersService,
    IApplicationDbContext dbContext)
    : IRequestHandler<GetCandidatesQuery, List<UserResponse>>
{
    public async Task<List<UserResponse>> Handle(GetCandidatesQuery request, CancellationToken cancellationToken)
    {
        var users = await usersService.GetUsersAsync(request.FullName, request.Email, request.PhoneNumber,
            request.Skills);

        var applicantUserNames = await dbContext.Applications.Select(a => a.ApplicantUsername).Distinct()
            .ToListAsync(cancellationToken);

        return users.Where(user => applicantUserNames.Contains(user.UserName, StringComparer.OrdinalIgnoreCase)).ToList();
    }
}