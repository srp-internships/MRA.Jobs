using MRA.Jobs.Application.Contracts.Dtos;

namespace MRA.Jobs.Application.Common.Interfaces;

public interface IVacancyTaskService
{
    public Task CheckVacancyTasksAsync(Guid applicationId, IEnumerable<TaskResponse> taskResponses,
        CancellationToken cancellationToken);
}