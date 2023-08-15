namespace MRA.Jobs.Domain.Entities;
public class InternshipVacancy : Vacancy
{
    public DateTime ApplicationDeadline { get; set; }
    public int Duration { get; set; }
    public int Stipend { get; set; }
}
