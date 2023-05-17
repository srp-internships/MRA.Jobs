using MediatR;

namespace MRA.Jobs.Application.Contracts.VacancyCategories.Queries;
public class GetVacancyCategoriesQuery : IRequest<List<Responces.VacancyCategoryListDTO>>
{

}
