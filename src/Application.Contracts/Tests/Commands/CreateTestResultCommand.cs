using MRA.Jobs.Application.Contracts.Common;

namespace MRA.Jobs.Application.Contracts.Tests.Commands;

public class CreateTestResultCommand : IRequest<TestResultDto>
{
    public Guid TestId { get; set; }
    public Guid UserId { get; set; }
    public string Slug { get; set; }
    public int Score { get; set; }
}