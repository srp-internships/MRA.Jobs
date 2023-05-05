using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MRA.Jobs.Domain.Entities;

namespace MRA.Jobs.Infrastructure.Persistence.Configurations;
public class ApplicationTimelineEventConfiguration : IEntityTypeConfiguration<ApplicationTimelineEvent>
{
    public void Configure(EntityTypeBuilder<ApplicationTimelineEvent> builder)
    {
        builder.ToTable(nameof(TimelineEvent));
        builder.Property(t => t.Id)
            .ValueGeneratedOnAdd();
        builder.HasOne(t => t.Application)
            .WithMany(t => t.applicationTimelineEvents)
            .HasForeignKey(t => t.ApplicationId);
    }
}
