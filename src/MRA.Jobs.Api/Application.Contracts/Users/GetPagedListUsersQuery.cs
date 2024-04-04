using MRA.Identity.Application.Contract.User.Responses;
using MRA.Jobs.Application.Contracts.Common;

namespace MRA.Jobs.Application.Contracts.Users;

public class GetPagedListUsersQuery : IRequest<PagedList<UserResponse>>
{
    public string Filters { get; set; }
    public string Sorts { get; set; }
    public int? Page { get; set; }
    public int? PageSize { get; set; }
    public string Skills { get; set; }
}