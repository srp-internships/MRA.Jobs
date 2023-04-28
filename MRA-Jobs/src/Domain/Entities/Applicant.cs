namespace MRA_Jobs.Domain.Entities;
public class Applicant
{
    public int Id { get; set; }
    public User User { get; set; }
    public ICollection<Application> Applications { get; set; }
}
