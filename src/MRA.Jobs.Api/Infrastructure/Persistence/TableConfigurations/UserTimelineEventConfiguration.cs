﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MRA.Jobs.Infrastructure.Persistence.TableConfigurations;

public class UserTimelineEventConfiguration : IEntityTypeConfiguration<UserTimelineEvent>
{
    public void Configure(EntityTypeBuilder<UserTimelineEvent> builder)
    {
        // No need to specify the properties here as they are already defined in the base TimelineEventConfiguration

        // Navigation properties

    }
}