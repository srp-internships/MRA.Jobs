using MRA.Jobs.Application.Contracts.Vacancies.Responses;

namespace MRA.Jobs.Application.Features.Vacancies;

public class VacancyProfile : Profile
{
    public VacancyProfile()
    {
        CreateMap<Vacancy, VacancyDto>();
    }
}