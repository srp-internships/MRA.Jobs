using MRA.Jobs.Application.Contracts.Applications.Responses;
using MRA.Jobs.Application.Contracts.Dtos.Enums;
using static MRA.Jobs.Application.Contracts.Dtos.Enums.ApplicationStatusDto;

namespace MRA.Jobs.Application.Contracts.Applications.Queries;

public class GetApplicationsByStatusQuery : IRequest<List<ApplicationListStatus>>
{
    public ApplicationStatus Status { get; set; }
}