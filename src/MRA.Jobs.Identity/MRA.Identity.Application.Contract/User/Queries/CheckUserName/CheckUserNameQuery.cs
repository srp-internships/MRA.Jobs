using MediatR;

namespace MRA.Identity.Application.Contract.User.Queries.CheckUserName;
public class CheckUserNameQuery : IRequest<bool>
{
    public string UserName { get; set; }
}
