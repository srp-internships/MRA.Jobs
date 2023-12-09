using MRA.Jobs.Application.Contracts.HiddenVacancies.Responses;

namespace MRA.Jobs.Application.Features.HiddenVacancies;

public class HiddenVacancyProfile : Profile
{
    public HiddenVacancyProfile()
    {
        CreateMap<HiddenVacancy, HiddenVacancyResponse>();
    }
}