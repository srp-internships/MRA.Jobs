
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MRA.Jobs.Domain.Entities;

namespace MRA.Jobs.Infrastructure.Persistence.Configurations;
public class UserTimelineEventConfiguration : IEntityTypeConfiguration<UserTimelineEvent>
{
    public void Configure(EntityTypeBuilder<UserTimelineEvent> builder)
    {
        builder.HasOne(t => t.User)
            .WithMany(t => t.userTimelineEvents)
            .HasForeignKey(t => t.UserId);
    }
}

public class TimelineEventConfiguration : IEntityTypeConfiguration<TimelineEvent>
{
    public void Configure(EntityTypeBuilder<TimelineEvent> builder)
    {
        builder.ToTable(nameof(TimelineEvent));
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id)
            .ValueGeneratedOnAdd();
    }
}
