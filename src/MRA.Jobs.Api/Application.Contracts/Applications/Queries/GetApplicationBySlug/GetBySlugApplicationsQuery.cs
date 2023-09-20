using MRA.Jobs.Application.Contracts.Applications.Responses;

namespace MRA.Jobs.Application.Contracts.Applications.Queries.GetApplicationBySlug;
public class GetBySlugApplicationQuery : IRequest<ApplicationDetailsDto>
{
    public string Slug { get; set; }
}
