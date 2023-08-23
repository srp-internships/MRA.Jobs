namespace MRA.Jobs.Domain.Common;

public interface ISoftDelete
{
    bool IsDeleted { get; set; }
}