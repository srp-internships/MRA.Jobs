namespace MRA_Jobs.Domain.Entities;
public class Status : BaseEntity
{
    public string Name { get; set; }
    public ICollection<Application> Applications { get; set; }
}