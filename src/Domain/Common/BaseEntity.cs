namespace MRA.Jobs.Domain.Common;

public abstract class BaseEntity : ISoftDelete
{
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; }
}