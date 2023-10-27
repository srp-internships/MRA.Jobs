namespace MRA.Jobs.Domain.Entities;
public class VacancyResponse
{
    public Guid Id { get; set; }
    public string Response { get; set; }
    public VacancyQuestion VacancyQuestion { get; set; }
}
