using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MRA.Identity.Domain.Entities;

namespace MRA.Identity.Infrastructure.Persistence.TableConfigurations;
public class ApplicationRoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
{
    public void Configure(EntityTypeBuilder<ApplicationRole> builder)
    {
        builder.Property(r => r.Slug).IsRequired();
        builder.HasIndex(r => r.Slug).IsUnique();
    }
}
