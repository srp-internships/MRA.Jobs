namespace MRA.Jobs.Infrastructure.Identity.Entities;

public class RolePermission
{
    public Guid Id { get; set; }

    public virtual ApplicationRole Role { get; set; }
    public Guid RoleId { get; set; }

    public virtual Permission Permission { get; set; }
    public Guid PermissionId { get; set; }
}