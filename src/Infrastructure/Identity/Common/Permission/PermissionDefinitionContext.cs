namespace MRA.Identity.Authorization;

public class PermissionDefinitionContext : IPermissionDefinitionContext
{
    public Dictionary<string, IPermissionGroupDefinition> Groups { get; }

    public PermissionDefinitionContext()
    {
        Groups = new Dictionary<string, IPermissionGroupDefinition>();
    }

    public virtual IPermissionGroupDefinition AddGroup(string name)
    {
        if (Groups.ContainsKey(name))
        {
            throw new Exception($"There is already an existing permission group with name: {name}");
        }

        return Groups[name] = new PermissionGroupDefinition(name);
    }

    public virtual IPermissionGroupDefinition GetGroup(string name)
    {
        var group = GetGroupOrNull(name);

        if (group == null)
        {
            throw new Exception($"Could not find a permission definition group with the given name: {name}");
        }

        return group;
    }

    public virtual IPermissionGroupDefinition GetGroupOrNull(string name)
    {
        if (!Groups.ContainsKey(name))
        {
            return null;
        }

        return Groups[name];
    }

    public virtual void RemoveGroup(string name)
    {
        if (!Groups.ContainsKey(name))
        {
            throw new Exception($"Not found permission group with name: {name}");
        }

        Groups.Remove(name);
    }

    public virtual IPermissionDefinition GetPermissionOrNull(string name)
    {
        foreach (var groupDefinition in Groups.Values)
        {
            var permissionDefinition = groupDefinition.Permissions.FirstOrDefault(p => p.Name == name);
            if (permissionDefinition != null)
            {
                return permissionDefinition;
            }
        }

        return null;
    }
}