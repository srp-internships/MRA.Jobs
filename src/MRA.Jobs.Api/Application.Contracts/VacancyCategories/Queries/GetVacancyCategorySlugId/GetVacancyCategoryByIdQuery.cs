using MRA.Jobs.Application.Contracts.VacancyCategories.Responses;

namespace MRA.Jobs.Application.Contracts.VacancyCategories.Queries.GetVacancyCategorySlugId;

public class GetVacancyCategoryByIdQuery : IRequest<CategoryResponse>
{
    public Guid Id { get; set; }
}