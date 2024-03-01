using MRA.Jobs.Application.Contracts.Applications.Responses;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.Dtos.Enums;

namespace MRA.Jobs.Application.Contracts.Applications.Queries.GetApplicationWithPagination;

public class GetApplicationsByFiltersQuery : PagedListQuery<ApplicationListDto>
{
    public string FullName { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string Skills { get; set; }
}