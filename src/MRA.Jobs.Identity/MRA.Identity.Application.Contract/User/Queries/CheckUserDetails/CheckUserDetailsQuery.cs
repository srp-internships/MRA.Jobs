using MediatR;
using MRA.Identity.Application.Contract.User.Responses;

namespace MRA.Identity.Application.Contract.User.Queries.CheckUserDetails;
public class CheckUserDetailsQuery : IRequest<UserDetailsResponse>
{
    public string UserName { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
}
