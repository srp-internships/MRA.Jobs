using MRA.Jobs.Application.Contracts.Vacncies.Responses;

namespace MRA.Jobs.Application.Features.Vacancies;
public class VacancyProfile : Profile
{
    public VacancyProfile()
    {
        CreateMap<Vacancy, VacancyListDTO>();
    }
}


