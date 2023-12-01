using Microsoft.EntityFrameworkCore;

namespace MRA.Jobs.Domain.Entities;

[Index(nameof(Slug))]
public class Application : BaseAuditableEntity
{
    public string CoverLetter { get; set; }
    public DateTime AppliedAt { get; set; } = DateTime.UtcNow;
    public ApplicationStatus Status { get; set; }
    public string ApplicantUsername { get; set; }
    public Vacancy Vacancy { get; set; }
    public Guid VacancyId { get; set; }
    public Guid ApplicantId { get; set; }

    public TestResult TestResult { get; set; }

    public ICollection<ApplicationTimelineEvent> History { get; set; }
    public string Slug { get; set; }
    public IEnumerable<VacancyResponse> VacancyResponses { get; set; }
    public IEnumerable<TaskResponse> TaskResponses { get; set; }

    public string CV { get; set; }
  
}