using MediatR;

namespace MRA.Identity.Application.Contract.User.Queries.GetUserNameByPhoneNymber;
public class IsAvailableUserPhoneNumberQuery : IRequest<bool>
{
    public string PhoneNumber { get; set; }
}
