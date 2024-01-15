using MRA.Jobs.Application.Contracts.TimeLineDTO;

namespace MRA.Jobs.Application.Contracts.Applications.Queries.GetApplicationNotes;

public class GetApplicationNotesQuery : IRequest<List<TimeLineDetailsDto>>
{
    public required string Slug { get; set; }
}