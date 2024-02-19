using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.Vacancies.Tags.Commands;

namespace MRA.Jobs.Application.Features.Vacancies.Tags;

public class RemoveTagsFromVacancyCommandHandler(IApplicationDbContext dbContext)
    : IRequestHandler<RemoveTagsFromVacancyCommand, List<string>>
{
    public async Task<List<string>> Handle(RemoveTagsFromVacancyCommand request, CancellationToken cancellationToken)
    {
        var vacancy = await dbContext.Vacancies.FirstOrDefaultAsync(v => v.Id == request.VacancyId, cancellationToken);
        _ = vacancy ?? throw new NotFoundException(nameof(vacancy), request.VacancyId);

        var vacancyTags = await dbContext.VacancyTags
            .Include(t=>t.Tag)
            .Where(t => t.VacancyId == request.VacancyId)
            .ToListAsync(cancellationToken);

        request.Tags = request.Tags.Distinct().ToArray();
        
        vacancyTags.RemoveAll(t => request.Tags.Contains(t.Tag.Name));

        await dbContext.SaveChangesAsync(cancellationToken);

        return vacancyTags.Select(t=>t.Tag.Name).ToList();
    }
}