namespace MRA.Jobs.Infrastructure.Identity.Common.Permission;

public class PermissionDefinition : IPermissionDefinition
{
    public string Name { get; }

    public bool IsEnabled { get; set; }

    public PermissionDefinition(string name, bool isEnabled)
    {
        Name = name;
        IsEnabled = isEnabled;
    }

    public override string ToString()
    {
        return $"[{nameof(PermissionDefinition)} {Name}]";
    }

}
