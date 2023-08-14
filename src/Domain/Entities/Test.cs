namespace MRA.Jobs.Domain.Entities;

public class Test : BaseAuditableEntity
{
    public string Title { get; set; }
    public string Description { get; set; }
    public TimeSpan Duration { get; set; }
    public long NumberOfQuestion { get; set; }
    public int PassingScore { get; set; }

    public Guid VacancyId { get; set; }
    public Vacancy Vacancy { get; set; }

    public ICollection<TestResult> Results { get; set; }
}
