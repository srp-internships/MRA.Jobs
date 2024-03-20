using Microsoft.EntityFrameworkCore;
using Sieve.Attributes;

namespace MRA.Jobs.Domain.Entities;

[Index(nameof(Slug))]
public abstract class Vacancy : BaseAuditableEntity
{
    public string CreatedByEmail { get; set; }
    public string Title { get; set; }
    public string ShortDescription { get; set; }
    public string Description { get; set; }
    public DateTime PublishDate { get; set; }
    public DateTime EndDate { get; set; }
    
    [Sieve(CanFilter = true, CanSort = true)]
    public string Slug { get; set; }

    public Guid CategoryId { get; set; }
    public VacancyCategory Category { get; set; }

    public ICollection<Application> Applications { get; set; }
    public ICollection<VacancyTimelineEvent> History { get; set; }
    public ICollection<Test> Tests { get; set; }
    public List<VacancyQuestion> VacancyQuestions { get; set; }
    public List<VacancyTask> VacancyTasks { get; set; }
    public IEnumerable<VacancyTag> Tags { get; set; }
    public string Discriminator { get; set; }
    public string Note { get; set; }
}

