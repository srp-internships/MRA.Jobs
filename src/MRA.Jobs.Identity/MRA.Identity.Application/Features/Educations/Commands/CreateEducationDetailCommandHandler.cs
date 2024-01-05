using MediatR;
using Microsoft.EntityFrameworkCore;
using MRA.Identity.Application.Common.Exceptions;
using MRA.Identity.Application.Common.Interfaces.DbContexts;
using MRA.Identity.Application.Common.Interfaces.Services;
using MRA.Identity.Application.Contract.Educations.Command.Create;
using MRA.Identity.Domain.Entities;

namespace MRA.Identity.Application.Features.Educations.Commands;

public class
    CreateEducationDetailCommandHandler(
        IApplicationDbContext context,
        IUserHttpContextAccessor userHttpContextAccessor)
    : IRequestHandler<CreateEducationDetailCommand, Guid>
{
    public async Task<Guid> Handle(CreateEducationDetailCommand request, CancellationToken cancellationToken)
    {
        var userId = userHttpContextAccessor.GetUserId();
        var user = await context.Users
            .Include(u => u.Educations)
            .FirstOrDefaultAsync(u => u.Id.Equals(userId), cancellationToken: cancellationToken);
        _ = user ?? throw new NotFoundException("user is not found");

        var textInfo = System.Globalization.CultureInfo.CurrentCulture.TextInfo;
        var university = textInfo.ToTitleCase(request.University.Trim());
        var speciality = textInfo.ToTitleCase(request.Speciality.Trim());

        var existingEducation = user.Educations.FirstOrDefault(e =>
            e.University.Equals(university, StringComparison.OrdinalIgnoreCase) &&
            e.Speciality.Equals(speciality, StringComparison.OrdinalIgnoreCase));

        if (existingEducation != null)
        {
            throw new ValidationException("Education detail already exists.");
        }

        var education = new EducationDetail
        {
            University = university,
            Speciality = speciality,
            StartDate = request.StartDate ?? default(DateTime),
            EndDate = request.EndDate ?? default(DateTime),
            UntilNow = request.UntilNow
        };

        user.Educations.Add(education);
        await context.SaveChangesAsync(cancellationToken);
        return education.Id;
    }
}