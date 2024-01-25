using MediatR;

namespace MRA.Identity.Application.Contract.User.Queries;
public class SendVerificationCodeSmsQuery : IRequest<bool>
{
    public string PhoneNumber { get; set; }
}
