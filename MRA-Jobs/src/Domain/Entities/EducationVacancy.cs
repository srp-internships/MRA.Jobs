namespace MRA_Jobs.Domain.Entities;

public class EducationVacancy : Vacancy
{
    public DateTime ClassStartDate { get; set; }
    public DateTime ClassEndDate { get; set; }
}