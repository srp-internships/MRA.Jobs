using MRA.Identity.Application.Contract.User.Queries;
using MRA.Identity.Application.Contract.User.Responses;
using MRA.Jobs.Application.Contracts.Applications.Candidates;
using MRA.Jobs.Application.Contracts.Common;

namespace MRA.Jobs.Application.Common.Interfaces;

public interface IUsersService
{
    Task<List<UserResponse>> GetUsersAsync(GetCandidatesQuery query);
    
}