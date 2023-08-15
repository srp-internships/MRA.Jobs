using Sieve.Services;

namespace MRA.Jobs.Application.Features.Applications;

public class SieveConfigurationForApplication : ISieveConfiguration
{
    public void Configure(SievePropertyMapper mapper)
    {
        mapper.Property<Domain.Entities.Application>(p => p.Applicant.FirstName)
            .CanFilter()
            .HasName(nameof(Domain.Entities.Application.Applicant));

        mapper.Property<Domain.Entities.Application>(p => p.ApplicantId)
            .CanFilter();

        mapper.Property<Domain.Entities.Application>(p => p.Vacancy.Title)
            .CanFilter()
            .HasName(nameof(Domain.Entities.Application.Vacancy));

        mapper.Property<Domain.Entities.Application>(p => p.VacancyId)
            .CanFilter();

        mapper.Property<Domain.Entities.Application>(p => p.CV)
            .CanFilter()
            .CanSort();

        mapper.Property<Domain.Entities.Application>(p => p.Status)
            .CanFilter()
            .CanSort();
    }
}