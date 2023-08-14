using MRA.Jobs.Application.Contracts.Applications.Responses;

namespace MRA.Jobs.Application.Contracts.Applications.Queries;
public class GetApplicationsByStatusQuery : IRequest<List<ApplicationListStatus>>
{
    public ApplicationStatus Status { get; set; }
}