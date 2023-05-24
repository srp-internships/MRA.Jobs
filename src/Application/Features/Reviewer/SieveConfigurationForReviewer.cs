using Sieve.Services;

namespace MRA.Jobs.Application.Features.Reviewer;
using MRA.Jobs.Domain.Entities;
public class SieveConfigurationForReviewer : ISieveConfiguration
{
    public void Configure(SievePropertyMapper mapper)
    {
        mapper.Property<Reviewer>(p => p.FirstName)
            .CanFilter()
            .CanSort();

        mapper.Property<Reviewer>(p => p.LastName)
            .CanFilter()
            .CanSort();

        mapper.Property<Reviewer>(p => p.Email)
            .CanFilter()
            .CanSort();

        mapper.Property<Reviewer>(p => p.PhoneNumber)
            .CanFilter()
            .CanSort();
    }
}
