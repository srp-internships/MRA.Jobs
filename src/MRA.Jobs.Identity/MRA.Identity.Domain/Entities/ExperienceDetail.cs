
namespace MRA.Identity.Domain.Entities;
public class ExperienceDetail
{
    public Guid Id { get; set; }
    public string JobTitle { get; set; }
    public string CompanyName { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime StartDate { get; set; }
    public bool IsCurrentJob { get; set; }
    public string Description { get; set; }
    public string Address { get; set; }

    public Guid UserId { get; set; }
    public ApplicationUser User { get; set; }
}
