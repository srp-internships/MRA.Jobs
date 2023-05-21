using MediatR;

namespace MRA.Jobs.Application.Contracts.VacancyCategories.Queries;
public class GetVacancyCategoryByIdQuery : IRequest<Responses.CategoryResponse>
{
    public Guid Id { get; set; }
}
