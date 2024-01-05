using MediatR;
using Microsoft.EntityFrameworkCore;
using MRA.Identity.Application.Common.Exceptions;
using MRA.Identity.Application.Common.Interfaces.DbContexts;
using MRA.Identity.Application.Common.Interfaces.Services;
using MRA.Identity.Application.Contract.Educations.Command.Update;

namespace MRA.Identity.Application.Features.Educations.Commands;

public class
    UpdateEducationDetailCommandHandler(
        IApplicationDbContext context,
        IUserHttpContextAccessor userHttpContextAccessor)
    : IRequestHandler<UpdateEducationDetailCommand, Guid>
{
    public async Task<Guid> Handle(UpdateEducationDetailCommand request, CancellationToken cancellationToken)
    {
        var userId = userHttpContextAccessor.GetUserId();
        var user = await context.Users
            .Include(u => u.Educations)
            .FirstOrDefaultAsync(u => u.Id.Equals(userId), cancellationToken: cancellationToken);
        _ = user ?? throw new NotFoundException("user is not found");

        var education = user.Educations.FirstOrDefault(e => e.Id.Equals(request.Id));
        _ = education ?? throw new NotFoundException("education not exits");

        var textInfo = System.Globalization.CultureInfo.CurrentCulture.TextInfo;
        var university = textInfo.ToTitleCase(request.University.Trim());
        var speciality = textInfo.ToTitleCase(request.Speciality.Trim());

        var existingEducation = user.Educations.FirstOrDefault(e =>
            e.Id != request.Id &&
            e.University.Equals(university, StringComparison.OrdinalIgnoreCase) &&
            e.Speciality.Equals(speciality, StringComparison.OrdinalIgnoreCase));

        if (existingEducation != null)
        {
            throw new ValidationException("Education detail already exists.");
        }

        education.University = university;
        education.Speciality = speciality;
        education.StartDate = request.StartDate ?? default(DateTime);
        education.EndDate = request.EndDate ?? default(DateTime);
        education.UntilNow = request.UntilNow;

        await context.SaveChangesAsync(cancellationToken);
        return education.Id;
    }
}