namespace MRA.Jobs.Domain.Entities;
public class VacancyTaskDetail
{
    public Guid Id { get; set; }
    public Guid ApplicantId { get; set; }
    public Guid TaskId { get; set; }
    public TaskSuccsess Success { get; set; }
    public string Codes { get; set; }
    public string Log { get; set; }
}
