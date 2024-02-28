using Microsoft.EntityFrameworkCore;
using Sieve.Attributes;

namespace MRA.Jobs.Domain.Entities;

[Index(nameof(Slug))]

public class Application : BaseAuditableEntity
{
    [Sieve(CanFilter = true, CanSort = true)]
    public string CoverLetter { get; set; }
    
    [Sieve(CanFilter = true, CanSort = true)]
    public DateTime AppliedAt { get; set; } = DateTime.UtcNow;
    
    public ApplicationStatus Status { get; set; }
    
    [Sieve(CanFilter = true, CanSort = true)]
    public string ApplicantUsername { get; set; }
    
    [Sieve(CanFilter = true, CanSort = true)]
    public Vacancy Vacancy { get; set; }
    public Guid VacancyId { get; set; }
    public Guid ApplicantId { get; set; }

    public TestResult TestResult { get; set; }

    public ICollection<ApplicationTimelineEvent> History { get; set; }
    
    [Sieve(CanFilter = true, CanSort = true)]
    public string Slug { get; set; }
    public IEnumerable<VacancyResponse> VacancyResponses { get; set; }
    public IEnumerable<TaskResponse> TaskResponses { get; set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public string CV { get; set; }
  
}