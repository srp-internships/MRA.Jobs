namespace MRA.Jobs.Domain.Entities;
public class VacancyTaskDetail
{
    public Guid Id { get; set; }
    public Guid ApplicantId { get; set; }
    public Guid TaskId { get; set; }
    public bool Success { get; init; }
    public string Codes { get; set; }

}
