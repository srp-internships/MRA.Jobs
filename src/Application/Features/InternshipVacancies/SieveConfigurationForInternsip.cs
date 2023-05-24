using Sieve.Services;

namespace MRA.Jobs.Application.Features.InternshipVacancies;
public class SieveConfigurationForInternsip : ISieveConfiguration
{
    public void Configure(SievePropertyMapper mapper)
    {
        mapper.Property<InternshipVacancy>(p => p.Category.Name)
            .CanFilter()
            .HasName(nameof(InternshipVacancy.Category));

        mapper.Property<InternshipVacancy>(p => p.CategoryId)
            .CanFilter();

        mapper.Property<InternshipVacancy>(p => p.Title)
            .CanFilter()
            .CanSort();

        mapper.Property<InternshipVacancy>(p => p.ShortDescription)
            .CanFilter()
            .CanSort();

        mapper.Property<InternshipVacancy>(p => p.PublishDate)
            .CanFilter()
            .CanSort();

        mapper.Property<InternshipVacancy>(p => p.EndDate)
            .CanFilter()
            .CanSort();

        mapper.Property<InternshipVacancy>(p => p.ApplicationDeadline)
            .CanFilter()
            .CanSort();

        mapper.Property<InternshipVacancy>(p => p.Duration)
            .CanFilter()
            .CanSort();

        mapper.Property<InternshipVacancy>(p => p.Stipend)
            .CanFilter()
            .CanSort();
    }
}
