using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MRA_Jobs.Domain.Entities;

namespace MRA_Jobs.Infrastructure.Persistence.Configurations;

public class EducationVacancyConfiguration : IEntityTypeConfiguration<EducationVacancy>
{
    public void Configure(EntityTypeBuilder<EducationVacancy> builder)
    {
        builder.ToTable(nameof(Vacancy));

        builder.HasKey(x => x.Id);

        builder.HasOne(e => e.Category)
            .WithMany(c => c.EducationVacancies)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasData(new EducationVacancy
        {
            Id = 5,
            Title = "Training class",
            Description = "tersd",
            ShortDescription = "sad"
        });
    }
}