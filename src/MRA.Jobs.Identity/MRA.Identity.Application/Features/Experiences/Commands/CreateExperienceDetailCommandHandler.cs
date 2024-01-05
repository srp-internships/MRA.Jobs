using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MRA.Identity.Application.Common.Exceptions;
using MRA.Identity.Application.Common.Interfaces.DbContexts;
using MRA.Identity.Application.Common.Interfaces.Services;
using MRA.Identity.Application.Contract.Experiences.Commands.Create;
using MRA.Identity.Domain.Entities;

namespace MRA.Identity.Application.Features.Experiences.Commands;

public class CreateExperienceDetailCommandHandler(
    IApplicationDbContext context,
    IUserHttpContextAccessor userHttpContextAccessor,
    IMapper mapper)
    : IRequestHandler<CreateExperienceDetailCommand, Guid>
{
    public async Task<Guid> Handle(CreateExperienceDetailCommand request, CancellationToken cancellationToken)
    {
        var userId = userHttpContextAccessor.GetUserId();
        var user = await context.Users
            .Include(u => u.Experiences)
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken: cancellationToken);
        _ = user ?? throw new NotFoundException("user is not found");

        var textInfo = System.Globalization.CultureInfo.CurrentCulture.TextInfo;
        var jobTitle = textInfo.ToTitleCase(request.JobTitle.Trim());
        var company = textInfo.ToTitleCase(request.CompanyName.Trim());
        request.Description = textInfo.ToTitleCase(request.Description.Trim());
        request.Address = request.Address.Trim();

        var existingExperience = user.Experiences.FirstOrDefault(e =>
            e.JobTitle.Equals(jobTitle, StringComparison.OrdinalIgnoreCase) &&
            e.CompanyName.Equals(company, StringComparison.OrdinalIgnoreCase));

        if (existingExperience != null)
        {
            throw new ValidationException("Experience detail already exists.");
        }

        var experienceDetail = mapper.Map<ExperienceDetail>(request);
        experienceDetail.JobTitle = jobTitle;
        experienceDetail.CompanyName = company;

        user.Experiences.Add(experienceDetail);
        await context.SaveChangesAsync(cancellationToken);
        return experienceDetail.Id;
    }
}