using MediatR;

namespace MRA.Jobs.Infrastructure.Shared.Users.Queries;

public class EmailExistQuery : IRequest<bool>
{
    public string Email { get; set; }
}