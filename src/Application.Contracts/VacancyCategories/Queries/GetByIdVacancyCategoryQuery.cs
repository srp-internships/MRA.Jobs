using MediatR;

namespace MRA.Jobs.Application.Contracts.VacancyCategories.Queries;
public class GetByIdVacancyCategoryQuery : IRequest<Responces.VacancyCategoryResponce>
{
    public Guid Id { get; set; }
}
