using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MRA.Jobs.Infrastructure.Persistence.TableConfigurations;

public class VacancyTimelineEventConfiguration : IEntityTypeConfiguration<VacancyTimelineEvent>
{
    public void Configure(EntityTypeBuilder<VacancyTimelineEvent> builder)
    {
        // No need to specify the properties here as they are already defined in the base TimelineEventConfiguration

        // Navigation properties
        builder.HasOne(vte => vte.Vacancy)
            .WithMany(v => v.History)
            .HasForeignKey(vte => vte.VacancyId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}