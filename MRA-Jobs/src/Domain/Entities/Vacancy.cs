namespace MRA_Jobs.Domain.Entities;
public abstract class Vacancy : BaseEntity
{
    public string Title { get; set; }
    public string ShortDescription { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime PublishDate { get; set; }
    public DateTime EndDate { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; }
}