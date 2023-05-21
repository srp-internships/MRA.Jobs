using System.Diagnostics.CodeAnalysis;

namespace MRA.Jobs.Application.Common.Security;

public interface IPermissionGroupDefinition
{
    public string Name { get; }

    public IReadOnlyList<IPermissionDefinition> Permissions { get; }

    public IPermissionDefinition AddPermission([NotNull] string name, bool isEnabled = true);
}
