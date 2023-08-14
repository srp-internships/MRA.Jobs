using System.Collections.Immutable;

namespace MRA.Jobs.Infrastructure.Identity.Authorization;

public class PermissionGroupDefinition : IPermissionGroupDefinition
{
    private readonly List<PermissionDefinition> _permissions;

    protected internal PermissionGroupDefinition(string name)
    {
        Name = name;
        _permissions = new List<PermissionDefinition>();
    }

    public string Name { get; }

    public IReadOnlyList<IPermissionDefinition> Permissions => _permissions.ToImmutableList();

    public virtual IPermissionDefinition AddPermission(string name, bool isEnabled = true)
    {
        PermissionDefinition permission = new PermissionDefinition(name, isEnabled);
        _permissions.Add(permission);
        return permission;
    }

    public override string ToString()
    {
        return $"[{nameof(PermissionGroupDefinition)} {Name}]";
    }
}