using System.ComponentModel.DataAnnotations.Schema;

namespace MRA_Jobs.Domain.Common;

public abstract class BaseEntity
{
    public long Id { get; set; }
}