using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MRA_Jobs.Domain.Entities;

namespace MRA_Jobs.Infrastructure.Persistence.Configurations;

public class RolesConfiguration : IEntityTypeConfiguration<Roles>
{
    public void Configure(EntityTypeBuilder<Roles> builder)
    {
        builder.ToTable(nameof(Roles));
        builder.HasData(new Roles()
        {
            Id = 1, 
            Name = "Role 1"
        });
    }
}