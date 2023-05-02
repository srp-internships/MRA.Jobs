namespace MRA.Jobs.Domain.Entities;

public class Applicant : BaseEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Patronymic { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public DateTime BirthDay { get; set; }

    public ICollection<Application> Applications { get; set; }
}
