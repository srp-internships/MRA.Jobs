using MRA.Jobs.Application.Contracts.VacancyTag.VacancyCategories.Responses;

namespace MRA.Jobs.Application.Contracts.VacancyTag.VacancyCategories.Queries;

public class GetVacancyCategoryByIdQuery : IRequest<CategoryResponse>
{
    public Guid Id { get; set; }
}