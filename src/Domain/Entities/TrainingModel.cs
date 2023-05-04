namespace MRA.Jobs.Domain.Entities;
public class TrainingModel : Vacancy
{
    public int Duration { get; set; }
    public DateTime EndDate { get; set; }
    public int Fees { get; set; }
    public DateTime StartDate { get; set; }
}
