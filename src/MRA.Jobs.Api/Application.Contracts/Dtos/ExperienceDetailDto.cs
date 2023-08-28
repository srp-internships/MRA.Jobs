namespace MRA.Jobs.Application.Contracts.Dtos;
public class ExperienceDetailDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string JobTitle { get; set; }
    public string CompanyName { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string JobLocationCity { get; set; }
    public string JobLocationCountry { get; set; }
    public string Description { get; set; }
    public bool IsCurrentJob { get; set; }
}
