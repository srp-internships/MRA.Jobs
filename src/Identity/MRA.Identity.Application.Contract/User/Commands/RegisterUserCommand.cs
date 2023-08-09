using MediatR;

namespace MRA.Identity.Application.Contract.User.Commands;

public class RegisterUserCommand:IRequest<Guid>
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
}