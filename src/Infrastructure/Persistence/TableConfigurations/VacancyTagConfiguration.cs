using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MRA.Jobs.Infrastructure.Persistence.TableConfigurations;

public class VacancyTagConfiguration : IEntityTypeConfiguration<VacancyTag>
{
    public void Configure(EntityTypeBuilder<VacancyTag> builder)
    {
        builder.HasOne(vt => vt.Tag)
            .WithMany(t => t.VacancyTags)
            .HasForeignKey(vt => vt.TagId);

        builder.HasOne(vt => vt.Vacancy)
            .WithMany(v => v.Tags)
            .HasForeignKey(vt => vt.VacancyId);
    }
}

