using MRA.Jobs.Application.Contracts.Dtos;

namespace MRA.Jobs.Application.Features.Vacancies.Responses;
public class VacancyResponseProfile : Profile
{
    public VacancyResponseProfile()
    {
        CreateMap<VacancyResponseDto, VacancyResponse>()
            .ForMember(dest => dest.VacancyQuestion, opt => opt.MapFrom(src => src.VacancyQuestion));

        CreateMap<VacancyResponse, VacancyResponseDto>();
    }
}
