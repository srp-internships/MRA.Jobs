using MediatR;

namespace MRA.Identity.Application.Contract.Admin.Commands;

public class RegisterAdminCommand:IRequest<ApplicationResponse<Guid>>
{
    public string Username { get; set; }
    public string Password { get; set; }
}