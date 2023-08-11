namespace MRA.Jobs.Domain.Entities;

public class Application : BaseAuditableEntity
{
    public string CoverLetter { get; set; }
    public string CV { get; set; }
    public DateTime AppliedAt { get; set; }
    public ApplicationStatus Status { get; set; }

    public Vacancy Vacancy { get; set; }
    public Guid VacancyId { get; set; }

    public Applicant Applicant { get; set; }
    public Guid ApplicantId { get; set; }

    public TestResult TestResult { get; set; }

    public ICollection<ApplicationTimelineEvent> History { get; set; }
}