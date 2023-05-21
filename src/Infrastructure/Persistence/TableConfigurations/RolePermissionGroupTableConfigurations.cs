using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MRA.Jobs.Infrastructure.Identity.Models;

namespace MRA.Jobs.Infrastructure.Persistence.TableConfigurations;

public class RolePermissionGroupTableConfigurations : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.ToTable(nameof(RolePermission));
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).ValueGeneratedOnAdd();

        builder.HasOne(e => e.Role)
            .WithMany()
            .HasForeignKey(e => e.RoleId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Permission)
            .WithMany()
            .HasForeignKey(e => e.PermissionId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
