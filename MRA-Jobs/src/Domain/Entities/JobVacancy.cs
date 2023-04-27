namespace MRA_Jobs.Domain.Entities;

public class JobVacancy : Vacancy
{
    public int RequeredYearOfExperience { get; set; }
    public decimal Salary { get; set; }
    public WorkSchedule WorkSchedule { get; set; }

}
public enum WorkSchedule
{
    FullTime = 1,
    Flexible
}
