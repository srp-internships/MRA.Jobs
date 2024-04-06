using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MRA.Identity.Application.Contract.User.Responses;
using MRA.Jobs.Application.Common.Sieve;
using MRA.Jobs.Application.Contracts.Applications.Queries.GetApplicationWithPagination;
using MRA.Jobs.Application.Contracts.Applications.Responses;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.Users;

namespace MRA.Jobs.Application.Features.Applications.Query.GetApplicationWithPagination;

public class GetApplicationsByFiltersQueryHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser,
    IApplicationSieveProcessor sieveProcessor,
    IMapper mapper,
    IMediator mediator)
    : IRequestHandler<GetApplicationsByFiltersQuery, PagedList<ApplicationListDto>>
{
    public async Task<PagedList<ApplicationListDto>> Handle(GetApplicationsByFiltersQuery request,
        CancellationToken cancellationToken)
    {
        var applications = dbContext.Applications
            .Include(a => a.Vacancy)
            .Include(a => a.VacancyResponses)
            .AsNoTracking();

        var roles = currentUser.GetRoles();
        if (roles.Any(r => r == "Reviewer"))
        {
            return await ReturnPagedListWithUsers(applications, request);
        }

        applications = applications.Where(a =>
            a.ApplicantUsername == currentUser.GetUserName() && a.Vacancy.Discriminator != "NoVacancy");

        return sieveProcessor.ApplyAdnGetPagedList(request, applications, mapper.Map<ApplicationListDto>);
    }

    private async Task<PagedList<ApplicationListDto>> ReturnPagedListWithUsers(
        IQueryable<Domain.Entities.Application> applications, GetApplicationsByFiltersQuery request)
    {
        var query = new GetListUsersQuery()
        {
            FullName = request.FullName,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email,
            Skills = request.Skills
        };
        var users = new List<UserResponse>();
        try
        {
            users = await mediator.Send(query);
        }
        catch (Exception)
        {
            //
        }


        if (!request.FullName.IsNullOrEmpty() || !request.Skills.IsNullOrEmpty() ||
            !request.PhoneNumber.IsNullOrEmpty() || !request.Email.IsNullOrEmpty())
        {
            applications = applications.Where(a => users.Select(u => u.UserName).Contains(a.ApplicantUsername));
        }

        var result = sieveProcessor.ApplyAdnGetPagedList(request, applications, mapper.Map<ApplicationListDto>);
        result.Items.ForEach(application =>
            application.User = users.FirstOrDefault(user => user.UserName == application.ApplicantUsername));

        return result;
    }
}