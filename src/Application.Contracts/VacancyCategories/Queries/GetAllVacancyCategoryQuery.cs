using MediatR;
using MRA.Jobs.Application.Contracts.VacancyCategories.Responces;

namespace MRA.Jobs.Application.Contracts.VacancyCategories.Queries;

public class GetAllVacancyCategoryQuery : IRequest<List<VacancyCategoryListDTO>>
{
}