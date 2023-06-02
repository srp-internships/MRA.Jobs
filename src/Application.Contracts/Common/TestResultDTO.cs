namespace MRA.Jobs.Application.Contracts.Common;
public class TestResultDTO
{
    public Guid UserId { get; set; }
    public Guid TestId { get; set; }
    public int Score { get; set; }
}
