namespace MRA.Jobs.Domain.Entities;
public class VacancyTask
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Template { get; set; }
    public string Test { get; set; }
}
