using MRA.Jobs.Application.Common.Mappings;
using MRA.Jobs.Domain.Entities;

namespace MRA.Jobs.Application.Common.Models;

// Note: This is currently just used to demonstrate applying multiple IMapFrom attributes.
public class LookupDto : IMapFrom<Applicant>
{
    public int Id { get; set; }

    public string? Title { get; set; }
}
