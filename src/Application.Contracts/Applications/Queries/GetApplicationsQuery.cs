using MediatR;
using MRA.Jobs.Application.Contracts.Applications.Responses;

namespace MRA.Jobs.Application.Contracts.Applications.Queries;
public class GetApplicationsQuery : IRequest<List<ApplicationListDTO>>
{
    public SieveQuery SieveQuery { get; set; }

    public GetApplicationsQuery(SieveQuery sieveQuery)
    {
        SieveQuery = sieveQuery;
    }
}
