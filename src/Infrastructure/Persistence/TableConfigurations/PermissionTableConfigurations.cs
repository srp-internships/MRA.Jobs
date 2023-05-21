using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MRA.Jobs.Infrastructure.Identity.Models;

namespace MRA.Jobs.Infrastructure.Persistence.TableConfigurations;

public class PermissionTableConfigurations : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable(nameof(Permission));
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).ValueGeneratedOnAdd();
    }
}
