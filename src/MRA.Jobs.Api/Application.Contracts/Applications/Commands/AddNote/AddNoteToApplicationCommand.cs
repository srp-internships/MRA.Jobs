using MRA.Jobs.Application.Contracts.TimeLineDTO;

namespace MRA.Jobs.Application.Contracts.Applications.Commands.AddNote;

public class AddNoteToApplicationCommand : IRequest<TimeLineDetailsDto>
{
    public string Slug { get; set; }
    public string Note { get; set; }
}