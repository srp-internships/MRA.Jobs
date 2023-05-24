namespace MRA.Jobs.Application.Common.Security;

public interface IPermissionDefinitionContext
{
    IPermissionGroupDefinition AddGroup(string name);
    IPermissionGroupDefinition GetGroup(string name);
    IPermissionGroupDefinition GetGroupOrNull(string name);
    IPermissionDefinition GetPermissionOrNull(string name);
    void RemoveGroup(string name);
}