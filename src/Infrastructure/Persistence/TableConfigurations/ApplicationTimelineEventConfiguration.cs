using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MRA.Jobs.Infrastructure.Persistence.TableConfigurations;

public class ApplicationTimelineEventConfiguration : IEntityTypeConfiguration<ApplicationTimelineEvent>
{
    public void Configure(EntityTypeBuilder<ApplicationTimelineEvent> builder)
    {
        // No need to specify the properties here as they are already defined in the base TimelineEventConfiguration

        // Navigation properties
        builder.HasOne(ate => ate.Application)
            .WithMany(a => a.History)
            .HasForeignKey(ate => ate.ApplicationId);
    }
}

