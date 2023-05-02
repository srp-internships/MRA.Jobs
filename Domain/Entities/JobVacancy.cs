using System.Text.Json.Serialization;

namespace MRA.Jobs.Domain.Entities;

public class JobVacancy : Vacancy
{
    public int RequiredYearOfExperience { get; set; }
    public WorkSchedule WorkSchedule { get; set; }

}