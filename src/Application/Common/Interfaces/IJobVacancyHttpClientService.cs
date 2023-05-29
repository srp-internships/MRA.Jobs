using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.JobVacancies.Commands;

namespace MRA.Jobs.Application.Common.Interfaces;
public interface IJobVacancyHttpClientService
{
    Task<TestInfoDTO> SendTestCreationRequest(CreateJobVacancyTestCommand request);
    Task<TestPassDTO> SendTestPassRequest(TestPassDTO request);
}
