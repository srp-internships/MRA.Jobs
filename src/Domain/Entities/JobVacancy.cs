using MRA.Jobs.Domain.Enums;

namespace MRA.Jobs.Domain.Entities;

public class JobVacancy : Vacancy
{
    public DateTime ApplicationDeadline { get; set; }
    public ExperienceLevel ExperienceLevel { get; set; }
    public JobType JobType { get; set; }
    public int SalaryRange { get; set; }
}