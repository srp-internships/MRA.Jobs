using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MRA.Jobs.Domain.Entities;

namespace MRA.Jobs.Infrastructure.Persistence.Configurations;
public class UserTimelineEventConfiguration : IEntityTypeConfiguration<UserTimelineEvent>
{
    public void Configure(EntityTypeBuilder<UserTimelineEvent> builder)
    {
        builder.ToTable(nameof(TimelineEvent));

    }
}
