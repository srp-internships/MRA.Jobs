namespace MRA.Jobs.Application.Contracts.Applications.Commands;
public class AddNoteToApplicationCommand : IRequest<bool>
{
    public Guid Id { get; set; }
    public string Note { get; set; }
}
