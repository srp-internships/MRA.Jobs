namespace MRA.Jobs.Application.Contracts.UserTag.Commands;
public class CreateUserTagCommand : IRequest<Guid>
{
    public Guid UserId { get; set; }
    public Guid TagId { get; set; }
}
