using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MRA.Jobs.Domain.Entities;

namespace MRA.Jobs.Infrastructure.Persistence.Configurations;
public class VacancyTimelineEventConfiguration : IEntityTypeConfiguration<VacancyTimelineEvent>
{
    public void Configure(EntityTypeBuilder<VacancyTimelineEvent> builder)
    {
        builder.ToTable(nameof(TimelineEvent));
        builder.Property(t => t.Id)
            .ValueGeneratedOnAdd();
        builder.HasOne(t => t.Vacancy)
            .WithMany(t=>t.VacancyTimelineEvents)
            .HasForeignKey(t => t.VacancyId);
    }
}
