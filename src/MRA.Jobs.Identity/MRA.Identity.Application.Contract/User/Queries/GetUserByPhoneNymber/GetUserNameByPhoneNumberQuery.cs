using MediatR;
using MRA.Identity.Application.Contract.User.Responses;

namespace MRA.Identity.Application.Contract.User.Queries.GetUserByPhoneNymber;
public class GetUserNameByPhoneNumberQuery : IRequest<string>
{
    public string PhoneNumber { get; set; }
}
