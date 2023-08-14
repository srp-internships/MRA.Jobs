namespace MRA.Jobs.Infrastructure.Identity.Authorization;

public class PermissionDefinition : IPermissionDefinition
{
    public PermissionDefinition(string name, bool isEnabled)
    {
        Name = name;
        IsEnabled = isEnabled;
    }

    public string Name { get; }

    public bool IsEnabled { get; set; }

    public override string ToString()
    {
        return $"[{nameof(PermissionDefinition)} {Name}]";
    }
}