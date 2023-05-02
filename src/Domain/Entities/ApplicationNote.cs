namespace MRA.Jobs.Domain.Entities;

public class ApplicationNote : BaseEntity
{
    public long AplicationId { get; set; }
    public string Description { get; set; }
    public DateTime Date { get; set; }
    public long? UserId { get; set; }
}
