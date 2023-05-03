namespace MRA.Jobs.Application.Contracts.JobVacancies.Queries;
public class GetJobVacancyDto
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string ShortDescription { get; set; }
    public string Description { get; set; }
    public DateTime PublishDate { get; set; }
    public DateTime EndDate { get; set; }
    public long CategoryId { get; set; }
    public int RequiredYearOfExperience { get; set; }
    public decimal Salary { get; set; }
    public int WorkSchedule { get; set; }
}