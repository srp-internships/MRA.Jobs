using MediatR;

namespace MRA.Jobs.Application.Contracts.VacancyCategories.Queries;
public class GetByIdVacancyCategoryQuery : IRequest<Responces.VacancyCategoryListDTO>
{
    public Guid Id { get; set; }
}
