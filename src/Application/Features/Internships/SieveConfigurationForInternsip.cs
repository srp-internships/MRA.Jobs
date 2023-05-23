using Sieve.Services;

namespace MRA.Jobs.Application.Features.Internships;
public class SieveConfigurationForInternsip : ISieveConfiguration
{
    public void Configure(SievePropertyMapper mapper)
    {
        mapper.Property<Internship>(p => p.Category.Name)
            .CanFilter()
            .HasName(nameof(Internship.Category));

        mapper.Property<Internship>(p => p.CategoryId)
            .CanFilter();

        mapper.Property<Internship>(p => p.Title)
            .CanFilter()
            .CanSort();

        mapper.Property<Internship>(p => p.ShortDescription)
            .CanFilter()
            .CanSort();

        mapper.Property<Internship>(p => p.PublishDate)
            .CanFilter()
            .CanSort();

        mapper.Property<Internship>(p => p.EndDate)
            .CanFilter()
            .CanSort();

        mapper.Property<Internship>(p => p.ApplicationDeadline)
            .CanFilter()
            .CanSort();

        mapper.Property<Internship>(p => p.Duration)
            .CanFilter()
            .CanSort();

        mapper.Property<Internship>(p => p.Stipend)
            .CanFilter()
            .CanSort();
    }
}
