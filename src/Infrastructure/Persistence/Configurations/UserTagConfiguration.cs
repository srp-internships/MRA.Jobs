using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MRA.Jobs.Domain.Entities;

namespace MRA.Jobs.Infrastructure.Persistence.Configurations;
public class UserTagConfiguration : IEntityTypeConfiguration<UserTag>
{
    public void Configure(EntityTypeBuilder<UserTag> builder)
    {
        builder.HasKey(ut => new { ut.UserId, ut.TagId });

        builder.HasOne(ut => ut.User)
            .WithMany(u => u.UserTags)
            .HasForeignKey(ut => ut.UserId);

        builder.HasOne(ut => ut.Tag)
            .WithMany(t => t.UserTags)
            .HasForeignKey(ut => ut.TagId);
    }
}
