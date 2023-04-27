using MRA_Jobs.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MRA_Jobs.Infrastructure.Persistence.Configurations;

public class JobVacancyConfiguration : IEntityTypeConfiguration<JobVacancy>
{
    public void Configure(EntityTypeBuilder<JobVacancy> builder)
    {
        builder.ToTable(nameof(Vacancy));
        builder.HasData(new JobVacancy() { Id = 3, Title = "Senior C# backend developer", Description = "tersd", ShortDescription = "sad" });

    }
}
