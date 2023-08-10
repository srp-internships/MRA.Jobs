namespace MRA.Jobs.Application.Common.Security;

public interface IPermissionDefinition
{
    public string Name { get; }

    public bool IsEnabled { get; set; }
}