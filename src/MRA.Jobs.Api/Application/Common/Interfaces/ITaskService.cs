using MRA.Jobs.Application.Contracts.Applications.Commands.CreateApplication;

namespace MRA.Jobs.Application.Common.Interfaces;
public interface ITaskService
{
    Task<List<VacancyTaskDetail>> GetGetVacancyTaskDetailAsync(CreateApplicationCommand application, CancellationToken cancellationToken);
}
