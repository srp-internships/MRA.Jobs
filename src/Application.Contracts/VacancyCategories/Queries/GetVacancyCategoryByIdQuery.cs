using MediatR;

namespace MRA.Jobs.Application.Contracts.VacancyCategories.Queries;
public class GetVacancyCategoryByIdQuery : IRequest<Responces.VacancyCategoryListDTO>
{
    public Guid Id { get; set; }
}
