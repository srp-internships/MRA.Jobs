using MRA.Identity.Application.Contract.User.Responses;

namespace MRA.Jobs.Application.Contracts.Users;

public class GetListUsersQuery : IRequest<List<UserResponse>>
{
    public string FullName { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string Skills { get; set; }
}