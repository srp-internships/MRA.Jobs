using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.Vacancies.Tags.Commands;

namespace MRA.Jobs.Application.Features.Vacancies.Tags;

public class DeleteTagsFromVacancyCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<DeleteTagsFromVacancyCommand, List<string>>
{
    public async Task<List<string>> Handle(DeleteTagsFromVacancyCommand request, CancellationToken cancellationToken)
    {
        var vacancy = await dbContext.Vacancies
            .Include(v => v.Tags)
            .ThenInclude(t => t.Tag)
            .FirstOrDefaultAsync(v => v.Id == request.VacancyId, cancellationToken);

        var tagsToRemove = vacancy.Tags.Where(t => request.Tags.Contains(t.Tag.Name)).ToList();

        foreach (var tag in tagsToRemove)
        {
            vacancy.Tags.Remove(tag);
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        return vacancy.Tags.Select(t => t.Tag.Name).ToList();
    }

}