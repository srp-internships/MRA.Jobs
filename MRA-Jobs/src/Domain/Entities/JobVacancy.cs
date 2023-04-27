namespace MRA_Jobs.Domain.Entities;

public class JobVacancy : Vacancy
{
    public int RequiredYearOfExperience { get; set; }
    public decimal Salary { get; set; }
    public WorkSchedule WorkSchedule { get; set; }

}