﻿using MRA.Jobs.Application.Contracts.VacancyCategories.Commands;

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
        var vacancyCategory = await _dbContext.Categories.FindAsync(new object[] { request.Id }, cancellationToken);
        if (vacancyCategory == null) 
            throw new NotFoundException(nameof(VacancyCategory),request.Id);

        _dbContext.Categories.Remove(vacancyCategory);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}
