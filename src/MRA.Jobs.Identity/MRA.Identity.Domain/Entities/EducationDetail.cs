namespace MRA.Identity.Domain.Entities;
public class EducationDetail
{
    public Guid Id { get; set; }
    public string University { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool UntilNow { get; set; }
    public string Speciality { get; set; }

    public Guid UserId { get; set; }
    public ApplicationUser User { get; set; }

}
