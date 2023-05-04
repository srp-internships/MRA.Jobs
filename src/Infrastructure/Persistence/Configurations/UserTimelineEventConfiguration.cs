
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MRA.Jobs.Domain.Entities;

namespace MRA.Jobs.Infrastructure.Persistence.Configurations;
public class UserTimelineEventConfiguration : IEntityTypeConfiguration<UserTimelineEvent>
{
    public void Configure(EntityTypeBuilder<UserTimelineEvent> builder)
    {
        builder.ToTable(nameof(TimelineEvent));
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id)
            .ValueGeneratedOnAdd();
        builder.HasOne(t => t.User)
            .WithMany(t => t.userTimelineEvents)
            .HasForeignKey(t => t.UserId);
    }
}
