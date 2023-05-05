using MRA.Jobs.Domain.Enums;

namespace MRA.Jobs.Domain.Entities;
public class Application : BaseAuditableEntity
{
    public Applicant Applicant { get; set; }
    public long ApplicantId { get; set; }

    public string CoverLetter { get; set; }

    public string History { get; set; } 

    public Vacancy Vacancy { get; set; }
    public long VacancyId { get; set; }
    public string ApplicantCvPath { get; set; }
    public string ApplicantAbout { get; set; }
    public DateTime ApplicationDate { get; set; }
    public long StatusId { get; set; }
    public string TestResult { get; set; }
    public ICollection<ApplicationTimelineEvent> TimelineEvents { get; set; }
}
