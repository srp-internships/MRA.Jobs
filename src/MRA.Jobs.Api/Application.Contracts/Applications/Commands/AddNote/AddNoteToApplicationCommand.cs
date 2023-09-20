namespace MRA.Jobs.Application.Contracts.Applications.Commands.AddNote;

public class AddNoteToApplicationCommand : IRequest<bool>
{
    public string Slug { get; set; }
    public string Note { get; set; }
}