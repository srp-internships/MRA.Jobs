using Sieve.Services;

namespace MRA.Jobs.Application.Features.JobVacancies;
public class SieveConfigurationForCategory : ISieveConfiguration
{
    public void Configure(SievePropertyMapper mapper)
    {
        mapper.Property<VacancyCategory>(p => p.Name)
            .CanFilter()
            .CanSort();
    }
}
