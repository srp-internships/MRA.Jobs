namespace MRA.Jobs.Application.Contracts.VacancyCategories.Queries.GetVacancyCategoryWithPagination;

public class GetVacancyCategoryBySlugQuery : IRequest<Responses.CategoryResponse>
{
    public string Slug { get; set; }
}
