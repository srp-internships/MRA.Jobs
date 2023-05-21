using System.Collections.Immutable;

namespace MRA.Jobs.Infrastructure.Identity.Common.Permission;

public class PermissionGroupDefinition : IPermissionGroupDefinition
{
    public string Name { get; }

    public IReadOnlyList<IPermissionDefinition> Permissions => _permissions.ToImmutableList();

    private readonly List<PermissionDefinition> _permissions;

    protected internal PermissionGroupDefinition(string name)
    {
        Name = name;
        _permissions = new List<PermissionDefinition>();
    }

    public virtual IPermissionDefinition AddPermission(string name, bool isEnabled = true)
    {
        var permission = new PermissionDefinition(name, isEnabled);
        _permissions.Add(permission);
        return permission;
    }

    public override string ToString()
    {
        return $"[{nameof(PermissionGroupDefinition)} {Name}]";
    }
}
