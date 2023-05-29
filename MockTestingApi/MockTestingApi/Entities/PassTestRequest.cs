namespace MockTestingApi.Entities;

public class PassTestRequest
{
    public Guid UserId { get; set; }
    public Guid TestId { get; set; }
    public int Score { get; set; }
}
