using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MRA.Jobs.Domain.Entities;

namespace MRA.Jobs.Infrastructure.Persistence.Configurations;

public class VacancyConfiguration : IEntityTypeConfiguration<Vacancy>
{
    public void Configure(EntityTypeBuilder<Vacancy> builder)
    {
        builder.ToTable(nameof(Vacancy));
        builder.HasKey(x => x.Id);
        builder.Property(p => p.Id).ValueGeneratedOnAdd();

        builder.Property(t => t.Title)
            .HasMaxLength(300)
            .IsRequired();

        builder.HasMany(v => v.Tags)
          .WithOne(vt => vt.Vacancy);
    }
}