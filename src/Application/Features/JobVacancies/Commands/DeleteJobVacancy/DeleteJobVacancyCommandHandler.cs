using MRA.Jobs.Application.Contracts.JobVacancies.Commands;

namespace MRA.Jobs.Application.Features.JobVacancies.Commands.DeleteJobVacancy;

public class DeleteJobVacancyCommandHandler : IRequestHandler<DeleteJobVacancyCommand, bool>
{
    private readonly IApplicationDbContext _dbContext;

    public DeleteJobVacancyCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> Handle(DeleteJobVacancyCommand request, CancellationToken cancellationToken)
    {
        var jobVacancy = await _dbContext.JobVacancies.FindAsync(new object[] { request.Id }, cancellationToken: cancellationToken);

        if (jobVacancy == null)
            throw new NotFoundException(nameof(JobVacancy), request.Id);

        _dbContext.JobVacancies.Remove(jobVacancy);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}