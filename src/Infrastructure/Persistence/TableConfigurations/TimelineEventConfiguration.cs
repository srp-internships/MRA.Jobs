using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MRA.Jobs.Infrastructure.Persistence.TableConfigurations;

public class TimelineEventConfiguration : IEntityTypeConfiguration<TimelineEvent>
{
    public void Configure(EntityTypeBuilder<TimelineEvent> builder)
    {
        builder.HasQueryFilter(e => !e.IsDeleted);
        builder.Property(te => te.CreateBy).HasColumnType("uniqueidentifier");
        builder.Property(te => te.Note).HasColumnType("nvarchar(max)");
        builder.Property(te => te.Time).HasColumnType("datetime2");
        builder.Property(te => te.EventType).HasColumnType("int");
    }
}

