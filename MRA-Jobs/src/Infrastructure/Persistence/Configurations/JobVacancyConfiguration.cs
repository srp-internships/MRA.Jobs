using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MRA_Jobs.Domain.Entities;

namespace MRA_Jobs.Infrastructure.Persistence.Configurations;

public class JobVacancyConfiguration : IEntityTypeConfiguration<JobVacancy>
{
    public void Configure(EntityTypeBuilder<JobVacancy> builder)
    {
        builder.ToTable(nameof(Vacancy));

        //builder.HasKey(j => j.Id);

        builder.HasOne(j => j.Category)
            .WithMany(c => c.JobVacancies)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasData(new JobVacancy()
        {
            Id = 3,
            Title = "Senior C# backend developer",
            Description = "tersd",
            ShortDescription = "sad"
        });
    }
}
