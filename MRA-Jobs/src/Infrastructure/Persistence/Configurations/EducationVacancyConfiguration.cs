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



        builder.HasData(new EducationVacancy() { Id = 5, Title = "Training class", Description = "tersd", ShortDescription = "sad" });
    }
}