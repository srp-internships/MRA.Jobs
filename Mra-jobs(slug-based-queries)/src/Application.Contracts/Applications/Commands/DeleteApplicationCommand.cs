namespace MRA.Jobs.Application.Contracts.Applications.Commands;
public class DeleteApplicationCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}
