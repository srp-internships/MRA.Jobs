using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.Tests.Commands;

namespace MRA.Jobs.Application.Common.Interfaces;

public interface ITestHttpClientService
{
    Task<TestInfoDto> SendTestCreationRequest(CreateTestCommand request);
    Task<TestResultDto> GetTestResultRequest(CreateTestResultCommand request);
}