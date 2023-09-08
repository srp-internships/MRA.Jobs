using MediatR;

namespace MRA.Identity.Application.Contract.User.Queries;
public class SendSmsQuery : IRequest<bool>
{
    public string PhoneNumber { get; set; }
}
