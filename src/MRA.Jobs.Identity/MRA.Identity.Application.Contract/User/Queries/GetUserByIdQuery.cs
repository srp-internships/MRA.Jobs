using MediatR;
using MRA.Identity.Application.Contract.User.Responses;

namespace MRA.Identity.Application.Contract.User.Queries;

public class GetUserByIdQuery : IRequest<UserResponse>
{
    public Guid Id { get; set; }
}