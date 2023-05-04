namespace MRA.Jobs.Application.Contracts.JobVacancies.Responses;

using MRA.Jobs.Domain.Entities;
public class JobVacancyFullResponse
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string ShortDescription { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime PublishDate { get; set; }
    public DateTime EndDate { get; set; }
    public long? CategoryId { get; set; }
    public VacancyCategory Category { get; set; }
    public ICollection<Application> Applications { get; set; }
    public int RequiredYearOfExperience { get; set; }
    public WorkSchedule WorkSchedule { get; set; }
}
