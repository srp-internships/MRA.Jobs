using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.Vacancies.Tags.Commands;

namespace MRA.Jobs.Application.Features.Vacancies.Tags;

public class AddTagsToVacancyCommandHandler(IApplicationDbContext dbContext)
    : IRequestHandler<AddTagsToVacancyCommand, List<string>>
{
    public async Task<List<string>> Handle(AddTagsToVacancyCommand request, CancellationToken cancellationToken)
    {
        var vacancy = await dbContext.Vacancies.FirstOrDefaultAsync(v => v.Id == request.VacancyId, cancellationToken);
        _ = vacancy ?? throw new NotFoundException(nameof(vacancy), request.VacancyId);
        
        var vacancyTags = await dbContext.VacancyTags
            .Include(t=>t.Tag)
            .Where(t => t.VacancyId == request.VacancyId)
            .ToListAsync(cancellationToken);
        
        request.Tags = request.Tags.Select(tag => tag.Trim()).Distinct(StringComparer.OrdinalIgnoreCase).ToArray();

        var tags = await dbContext.Tags.ToListAsync(cancellationToken);

        var newTags = request.Tags.Except(vacancyTags.Select(t => t.Tag.Name), StringComparer.OrdinalIgnoreCase);

        foreach (var tag in newTags)
        {
            var existingTag = tags.FirstOrDefault(t => t.Name.Equals(tag, StringComparison.OrdinalIgnoreCase));

            if (existingTag == null)
            {
                existingTag = new Tag() { Name = tag };
                dbContext.Tags.Add(existingTag);
            }

            vacancyTags.Add(new VacancyTag() { Tag = existingTag });
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        return vacancyTags.Select(t => t.Tag.Name).ToList();
    }
}