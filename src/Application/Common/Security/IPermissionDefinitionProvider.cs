namespace MRA.Jobs.Application.Common.Security;

public interface IPermissionDefinitionProvider
{
    void Define(IPermissionDefinitionContext context);
}