using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.Vacancies.Tags.Commands;

namespace MRA.Jobs.Application.Features.Vacancies.Tags;

public class RemoveTagsFromVacancyCommandHandler(IApplicationDbContext dbContext)
    : IRequestHandler<RemoveTagsFromVacancyCommand, List<string>>
{
    public async Task<List<string>> Handle(RemoveTagsFromVacancyCommand request, CancellationToken cancellationToken)
    {

        request.Tags = request.Tags.Distinct().ToArray();

        var tags = await dbContext.VacancyTags.Include(t => t.Tag).Where(t => t.VacancyId == request.VacancyId).ToListAsync();
        var tagsToRemove = tags.Where(t => request.Tags.Contains(t.Tag.Name)).ToList();

        foreach (var tag in tagsToRemove)
        {
            tags.Remove(tag);
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        return tags.Select(t => t.Tag.Name).ToList();
    }
}