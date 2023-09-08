using MediatR;

namespace MRA.Identity.Application.Contract.User.Queries;
public class SendSmsQuery : IRequest<string>
{
    public string PhoneNumber { get; set; }
}
