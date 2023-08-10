using MediatR;

namespace MRA.Jobs.Infrastructure.Shared.Users.Queries;

public class GetUserRolesQuery : IRequest<IEnumerable<string>>
{
    public Guid Id { get; set; }
}
