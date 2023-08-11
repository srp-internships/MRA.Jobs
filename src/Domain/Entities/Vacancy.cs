namespace MRA.Jobs.Domain.Entities;

public abstract class Vacancy : BaseAuditableEntity
{
    public string Title { get; set; }
    public string ShortDescription { get; set; }
    public string Description { get; set; }
    public DateTime PublishDate { get; set; }
    public DateTime EndDate { get; set; }

    public Guid CategoryId { get; set; }
    public VacancyCategory Category { get; set; }

    public ICollection<Application> Applications { get; set; }
    public ICollection<VacancyTimelineEvent> History { get; set; }
    public ICollection<Test> Tests { get; set; }

    public ICollection<VacancyTag> Tags { get; set; }
}