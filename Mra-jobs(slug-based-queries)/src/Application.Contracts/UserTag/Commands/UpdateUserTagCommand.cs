namespace MRA.Jobs.Application.Contracts.UserTag.Commands;
public class UpdateUserTagCommand : IRequest<Guid>
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid TagId { get; set; }
}
