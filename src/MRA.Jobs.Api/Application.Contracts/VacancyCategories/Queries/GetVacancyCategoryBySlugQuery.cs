namespace MRA.Jobs.Application.Contracts.VacancyCategories.Queries;

public class GetVacancyCategoryBySlugQuery : IRequest<Responses.CategoryResponse>
{
    public string Slug { get; set; }
}
