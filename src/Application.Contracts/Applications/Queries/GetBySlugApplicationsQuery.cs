using MRA.Jobs.Application.Contracts.Applications.Responses;

namespace MRA.Jobs.Application.Contracts.Applications.Queries;
public class GetBySlugApplicationQuery : IRequest<ApplicationDetailsDTO>
{
    public string Slug { get; set; }
}
