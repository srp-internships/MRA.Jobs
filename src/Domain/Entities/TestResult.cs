namespace MRA.Jobs.Domain.Entities;

public class TestResult : BaseAuditableEntity
{
    public DateTime CompletedAt { get; set; }
    public bool Passed { get; set; }
    public int Score { get; set; }

    public Guid TestId { get; set; }
    public Test Test { get; set; }

    public Guid ApplicationId { get; set; }
    public Application Application { get; set; }
}
