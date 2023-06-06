using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.Tests.Commands;

namespace MRA.Jobs.Application.Common.Interfaces;
public interface ITestHttpClientService
{
    Task<TestInfoDTO> SendTestCreationRequest(CreateTestCommand request);
    Task<TestResultDTO> GetTestResultRequest(CreateTestResultCommand request);
}
