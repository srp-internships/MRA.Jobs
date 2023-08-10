namespace MRA.Jobs.Domain.Entities;

public class VacancyTimelineEvent : TimelineEvent
{
    public Guid VacancyId { get; set; }
    public Vacancy Vacancy { get; set; }
}
