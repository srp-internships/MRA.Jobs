using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MRA.Jobs.Domain.Entities;

namespace MRA.Jobs.Infrastructure.Persistence.Configurations;
public class TimelineEventConfiguration : IEntityTypeConfiguration<TimelineEvent>
{
    public void Configure(EntityTypeBuilder<TimelineEvent> builder)
    {
        builder.ToTable(nameof(TimelineEvent));
        builder.HasDiscriminator<string>(nameof(TimelineEvent));
        builder.HasKey(t => t.Id);
    }
}
