using Sieve.Services;

namespace MRA.Jobs.Infrastructure.Identity.Features.Permissions;

internal class PermissionSieveConfiguration : ISieveConfiguration
{
    public void Configure(SievePropertyMapper mapper)
    {
        mapper.Property<Permission>(p => p.Name)
            .CanFilter()
            .CanSort();

        mapper.Property<Permission>(p => p.Group)
            .CanFilter()
            .CanSort();
    }
}