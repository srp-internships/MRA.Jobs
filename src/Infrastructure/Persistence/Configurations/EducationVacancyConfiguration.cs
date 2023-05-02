using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MRA.Jobs.Domain.Entities;

namespace MRA.Jobs.Infrastructure.Persistence.Configurations;

public class EducationVacancyConfiguration : IEntityTypeConfiguration<EducationVacancy>
{
    public void Configure(EntityTypeBuilder<EducationVacancy> builder)
    {
        builder.ToTable(nameof(Vacancy));

        builder.HasOne(e => e.Category)
            .WithMany(c => c.EducationVacancies)
            .HasForeignKey(e => e.CategoryId)
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