namespace MRA.Jobs.Application.Contracts.Common;

public class TestResultDto
{
    public Guid UserId { get; set; }
    public Guid TestId { get; set; }
    public int Score { get; set; }
}