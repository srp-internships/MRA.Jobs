using MRA.Jobs.Application.Contracts.VacancyCategories.Responses;

namespace MRA.Jobs.Application.Contracts.JobVacancies.Responses;
public class JobCategoriesResponse
{
    public Guid CategoryId { get; set; }
    public CategoryResponse Category { get; set; }
    public bool Selected { get; set; }
    public int JobsCount {  get; set; }
}
