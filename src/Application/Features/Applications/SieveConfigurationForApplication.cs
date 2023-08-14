using Sieve.Services;

namespace MRA.Jobs.Application.Features.Applications;
using MRA.Jobs.Domain.Entities;
public class SieveConfigurationForApplication : ISieveConfiguration
{
    public void Configure(SievePropertyMapper mapper)
    {
        mapper.Property<Application>(p => p.Applicant.FirstName)
            .CanFilter()
            .HasName(nameof(Application.Applicant));

        mapper.Property<Application>(p => p.ApplicantId)
            .CanFilter();

        mapper.Property<Application>(p => p.Vacancy.Title)
            .CanFilter()
            .HasName(nameof(Application.Vacancy));

        mapper.Property<Application>(p => p.VacancyId)
            .CanFilter();

        mapper.Property<Application>(p => p.CV)
            .CanFilter()
            .CanSort();

        mapper.Property<Application>(p => p.Status)
            .CanFilter()
            .CanSort();
    }
}
