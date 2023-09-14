using MediatR;

namespace MRA.Identity.Application.Contract.User.Queries;
public class SmsVerificationCodeCheckQuery : IRequest<bool>
{
    public string PhoneNumber { get; set; }
    public int Code { get; set; }
}
