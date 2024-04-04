using MRA.Identity.Application.Contract.User.Responses;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.Users;

namespace MRA.Jobs.Client.Services.Users;

public interface IUsersService
{
    Task<PagedList<UserResponse>> GetPagedListUsers(GetPagedListUsersQuery query, string url = null);
    Task<List<UserResponse>> GetListUsers(GetListUsersQuery query);
}