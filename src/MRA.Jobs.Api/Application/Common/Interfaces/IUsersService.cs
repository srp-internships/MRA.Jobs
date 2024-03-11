using MRA.Identity.Application.Contract.User.Queries;
using MRA.Identity.Application.Contract.User.Responses;

namespace MRA.Jobs.Application.Common.Interfaces;

public interface IUsersService
{
    Task<List<UserResponse>> GetUsersAsync(string fullName = null, string email = null, string phoneNumber = null,
        string skills = null);
}