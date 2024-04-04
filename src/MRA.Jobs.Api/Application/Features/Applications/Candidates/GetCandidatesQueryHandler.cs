using Microsoft.EntityFrameworkCore;
using MRA.Identity.Application.Contract.User.Responses;
using MRA.Jobs.Application.Contracts.Applications.Candidates;
using MRA.Jobs.Application.Contracts.Users;

namespace MRA.Jobs.Application.Features.Applications.Candidates;

public class GetCandidatesQueryHandler(
   IMediator mediator,
    IApplicationDbContext dbContext)
    : IRequestHandler<GetCandidatesQuery, List<UserResponse>>
{
    public async Task<List<UserResponse>> Handle(GetCandidatesQuery request, CancellationToken cancellationToken)
    {
        var users = await mediator.Send(new GetListUsersQuery()
        {
            FullName = request.FullName,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email,
            Skills = request.Skills
        });

        var applicantUserNames = await dbContext.Applications.Select(a => a.ApplicantUsername).Distinct()
            .ToListAsync(cancellationToken);

        return users.Where(user => applicantUserNames.Contains(user.UserName, StringComparer.OrdinalIgnoreCase))
            .ToList();
    }
}