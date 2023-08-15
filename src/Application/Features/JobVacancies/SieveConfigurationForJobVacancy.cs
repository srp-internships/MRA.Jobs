using Sieve.Services;

namespace MRA.Jobs.Application.Features.JobVacancies;

public class SieveConfigurationForJobVacancy : ISieveConfiguration
{
    public void Configure(SievePropertyMapper mapper)
    {
        mapper.Property<JobVacancy>(p => p.Title)
            .CanFilter()
            .CanSort();

        mapper.Property<JobVacancy>(p => p.Category.Name)
            .CanFilter()
            .HasName(nameof(JobVacancy.Category));

        mapper.Property<JobVacancy>(p => p.CategoryId)
            .CanFilter();

        mapper.Property<JobVacancy>(p => p.ShortDescription)
            .CanFilter()
            .CanSort();

        mapper.Property<JobVacancy>(p => p.PublishDate)
            .CanFilter()
            .CanSort();

        mapper.Property<JobVacancy>(p => p.EndDate)
            .CanFilter()
            .CanSort();

        mapper.Property<JobVacancy>(p => p.WorkSchedule)
            .CanFilter()
            .CanSort();
    }
}