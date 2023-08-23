using MediatR;
using MRA.Identity.Application.Contract.Admin.Responses;

namespace MRA.Identity.Application.Contract.Admin.Commands;

public class LoginAdminCommand:IRequest<ApplicationResponse<JwtTokenResponse>>
{
    public string Username { get; set; }
    public string Password { get; set; }
}