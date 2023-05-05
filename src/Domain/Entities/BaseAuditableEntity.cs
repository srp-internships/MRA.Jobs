namespace MRA.Jobs.Domain.Entities;

public abstract class BaseAuditableEntity : BaseEntity
{
    public DateTime CreatedAt { get; set; }

    public long? CreatedBy { get; set; }

    public DateTime? LastModifiedAt { get; set; }

    public long? LastModifiedBy { get; set; }
}
