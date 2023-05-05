namespace MRA.Jobs.Domain.Entities;

public class VacancyTimelineEvent : TimelineEvent
{
    public long VacancyId { get; set; }
    public Vacancy Vacancy { get; set; }
}
