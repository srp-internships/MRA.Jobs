using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.Vacancies.Tags.Commands;

namespace MRA.Jobs.Application.Features.Vacancies.Tags;

public class AddTagsToVacancyCommandHandler(IApplicationDbContext dbContext)
    : IRequestHandler<AddTagsToVacancyCommand, bool>
{
    public async Task<bool> Handle(AddTagsToVacancyCommand request, CancellationToken cancellationToken)
    {
        var vacancy = await dbContext.Vacancies.Include(v => v.Tags)
            .ThenInclude(t => t.Tag)
            .FirstOrDefaultAsync(v => v.Id == request.VacancyId, cancellationToken);
        _ = vacancy ?? throw new NotFoundException(nameof(vacancy), request.VacancyId);

        var newTags = request.Tags.Except(vacancy.Tags.Select(t => t.Tag.Name));
        
        foreach (var tag in newTags)
        {
            vacancy.Tags.Add(new VacancyTag() { Tag = new Tag() { Name = tag } });
        }

        return true;
    }
}