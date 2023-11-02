using MediatR;

namespace MRA.Identity.Application.Contract.User.Commands.ResetPassword;
public class ResetPasswordCommand : IRequest<bool>
{
    public int Code { get; set; }
    public string PhoneNumber { get; set; }
    public string Password { get; set; }
}
