namespace MRA.Jobs.Application.Contracts.Applications.Commands;

public class AddNoteToApplicationCommand : IRequest<bool>
{
    public string Slug { get; set; }
    public string Note { get; set; }
}