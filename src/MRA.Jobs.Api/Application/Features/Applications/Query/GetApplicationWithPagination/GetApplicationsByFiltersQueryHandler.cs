﻿using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MRA.Jobs.Application.Common.Sieve;
using MRA.Jobs.Application.Contracts.Applications.Candidates;
using MRA.Jobs.Application.Contracts.Applications.Queries.GetApplicationWithPagination;
using MRA.Jobs.Application.Contracts.Applications.Responses;
using MRA.Jobs.Application.Contracts.Common;

namespace MRA.Jobs.Application.Features.Applications.Query.GetApplicationWithPagination;

public class GetApplicationsByFiltersQueryHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUser,
    IApplicationSieveProcessor sieveProcessor,
    IMapper mapper,
    IUsersService usersService)
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
        var users =
            await usersService.GetUsersAsync(new GetCandidatesQuery()
            {
                FullName = request.FullName,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                Skills = request.Skills
            });

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