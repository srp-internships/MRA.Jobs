namespace MRA_Jobs.Domain.Entities;
public class Applicant:BaseEntity
{
    public User User { get; set; }
    public ICollection<Application> Applications { get; set; }
}
