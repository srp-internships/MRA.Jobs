using Microsoft.EntityFrameworkCore;
using MRA.Jobs.Application.Contracts.VacancyCategories.Commands;

namespace MRA.Jobs.Application.Features.VacancyCategories.Command.DeleteVacancyCategory;

public class DeleteVacancyCategoryCommandHandler : IRequestHandler<DeleteVacancyCategoryCommand, bool>
{
    private readonly IApplicationDbContext _dbContext;

    public DeleteVacancyCategoryCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> Handle(DeleteVacancyCategoryCommand request, CancellationToken cancellationToken)
    {
        var vacancyCategory = await _dbContext.Categories.FirstOrDefaultAsync(v => v.Slug == request.Slug, cancellationToken);
        if (vacancyCategory == null)
            throw new NotFoundException(nameof(VacancyCategory), request.Slug);

        _dbContext.Categories.Remove(vacancyCategory);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}
