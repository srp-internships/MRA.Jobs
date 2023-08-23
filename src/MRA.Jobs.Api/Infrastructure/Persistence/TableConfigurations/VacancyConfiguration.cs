using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MRA.Jobs.Infrastructure.Persistence.TableConfigurations;

public class VacancyConfiguration : IEntityTypeConfiguration<Vacancy>
{
    public void Configure(EntityTypeBuilder<Vacancy> builder)
    {
        builder.HasQueryFilter(e => !e.IsDeleted);
        // Common properties
        builder.Property(v => v.Title).HasColumnType("nvarchar(256)").IsRequired();
        builder.Property(v => v.ShortDescription).HasColumnType("nvarchar(max)");
        builder.Property(v => v.Description).HasColumnType("nvarchar(max)").IsRequired();
        builder.Property(v => v.PublishDate).HasColumnType("datetime2");
        builder.Property(v => v.EndDate).HasColumnType("datetime2");

        // Navigation properties
        builder.HasOne(v => v.Category)
            .WithMany(vc => vc.Vacancies)
            .HasForeignKey(v => v.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(v => v.Applications)
            .WithOne(a => a.Vacancy)
            .HasForeignKey(a => a.VacancyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(v => v.History)
            .WithOne(vte => vte.Vacancy)
            .HasForeignKey(vte => vte.VacancyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(v => v.Tags)
            .WithOne(vt => vt.Vacancy)
            .HasForeignKey(vt => vt.VacancyId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}