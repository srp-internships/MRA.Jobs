using Sieve.Services;

namespace MRA.Jobs.Application.Features.Reviewer;

public class SieveConfigurationForReviewer : ISieveConfiguration
{
    public void Configure(SievePropertyMapper mapper)
    {
        mapper.Property<Domain.Entities.Reviewer>(p => p.FirstName)
            .CanFilter()
            .CanSort();

        mapper.Property<Domain.Entities.Reviewer>(p => p.LastName)
            .CanFilter()
            .CanSort();

        mapper.Property<Domain.Entities.Reviewer>(p => p.Email)
            .CanFilter()
            .CanSort();

        mapper.Property<Domain.Entities.Reviewer>(p => p.PhoneNumber)
            .CanFilter()
            .CanSort();
    }
}