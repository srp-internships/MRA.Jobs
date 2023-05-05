using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MRA.Jobs.Infrastructure.Persistence.TableConfigurations;

public class UserTagConfiguration : IEntityTypeConfiguration<UserTag>
{
    public void Configure(EntityTypeBuilder<UserTag> builder)
    {
        builder.HasOne(ut => ut.Tag)
            .WithMany(t => t.UserTags)
            .HasForeignKey(ut => ut.TagId);

        builder.HasOne(ut => ut.User)
            .WithMany(u => u.Tags)
            .HasForeignKey(ut => ut.UserId);

        builder.HasIndex(ut => new { ut.TagId, ut.UserId }).IsUnique();
    }
}

