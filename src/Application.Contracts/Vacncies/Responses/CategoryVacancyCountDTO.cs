namespace MRA.Jobs.Application.Contracts.Vacncies.Responses;
public class CategoryVacancyCountDTO
{
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; }
    public int VacancyCount { get; set; }
}