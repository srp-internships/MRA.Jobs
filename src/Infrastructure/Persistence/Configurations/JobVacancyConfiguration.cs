using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MRA.Jobs.Domain.Entities;

namespace MRA.Jobs.Infrastructure.Persistence.Configurations;

public class JobVacancyConfiguration : IEntityTypeConfiguration<JobVacancy>
{
    public void Configure(EntityTypeBuilder<JobVacancy> builder)
    {
        builder.ToTable(nameof(Vacancy));

        builder.HasData(new JobVacancy()
        {
            Id = 3,
            Title = "Senior C# backend developer",
            Description = "tersd",
            ShortDescription = "sad"
        });
    }
}
