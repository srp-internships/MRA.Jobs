namespace MRA.Jobs.Application.Contracts.TimeLineDTO;

public class TimeLineDetailsDto
{
    public string CreateBy { get; set; }
    public string Note { get; set; }
    public DateTime Time { get; set; }
    public Dtos.Enums.ApplicationStatusDto.TimelineEventType EventType { get; set; }

}