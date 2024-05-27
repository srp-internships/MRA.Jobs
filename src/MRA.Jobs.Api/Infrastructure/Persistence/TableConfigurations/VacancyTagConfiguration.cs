using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MRA.Jobs.Infrastructure.Persistence.TableConfigurations;

public class VacancyTagConfiguration : IEntityTypeConfiguration<VacancyTag>
{
    public void Configure(EntityTypeBuilder<VacancyTag> builder)
    {
        builder.HasQueryFilter(e => !e.IsDeleted);
        builder.HasOne(vt => vt.Tag)
            .WithMany(t => t.VacancyTags)
            .HasForeignKey(vt => vt.TagId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(vt => vt.Vacancy)
            .WithMany(v => v.Tags)
            .HasForeignKey(vt => vt.VacancyId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}