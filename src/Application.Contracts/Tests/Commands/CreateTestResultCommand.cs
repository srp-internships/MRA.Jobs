using MRA.Jobs.Application.Contracts.Common;

namespace MRA.Jobs.Application.Contracts.Tests.Commands;
public class CreateTestResultCommand : IRequest<TestResultDTO>
{
    public Guid TestId { get; set; }
    public Guid UserId { get; set; }
    public int Score { get; set; }
}
