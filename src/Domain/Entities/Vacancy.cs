namespace MRA.Jobs.Domain.Entities;
public abstract class Vacancy : BaseEntity
{
    public string Title { get; set; }
    public string ShortDescription { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime PublishDate { get; set; }
    public DateTime EndDate { get; set; }
    public long? CategoryId { get; set; }
    public VacancyCategory Category { get; set; }

    public ICollection<Application> Applications { get; set; }
    public ICollection<VacancyTag> VacancyTags { get; set; }
}