using Sieve.Services;

namespace MRA.Jobs.Application.Features.Applicant;
using MRA.Jobs.Domain.Entities;
public class SieveConfigurationForApplicant : ISieveConfiguration
{
    public void Configure(SievePropertyMapper mapper)
    {
        mapper.Property<Applicant>(p => p.Id)
            .CanFilter()
            .CanSort();

        mapper.Property<Applicant>(p => p.FirstName)
            .CanFilter()
            .CanSort();

        mapper.Property<Applicant>(p => p.LastName)
            .CanFilter()
            .CanSort();

        mapper.Property<Applicant>(p => p.Email)
            .CanFilter()
            .CanSort();

        mapper.Property<Applicant>(p => p.PhoneNumber)
            .CanFilter()
            .CanSort();
    }
}
