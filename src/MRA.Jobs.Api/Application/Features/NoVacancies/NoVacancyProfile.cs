using MRA.Jobs.Application.Contracts.NoVacancies.Responses;

namespace MRA.Jobs.Application.Features.NoVacancies;

public class NoVacancyProfile : Profile
{
    public NoVacancyProfile()
    {
        CreateMap<NoVacancy, NoVacancyResponse>();
    }
}