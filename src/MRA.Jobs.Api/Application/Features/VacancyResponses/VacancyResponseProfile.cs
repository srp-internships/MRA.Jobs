using MRA.Jobs.Application.Contracts.Dtos;

namespace MRA.Jobs.Application.Features.VacancyResponses;
public class VacancyResponseProfile : Profile
{
    public VacancyResponseProfile()
    {
        CreateMap<VacancyResponseDto, VacancyResponse>()
            .ForMember(dest => dest.Question, opt => opt.MapFrom(src => src.Question));

        CreateMap<VacancyResponse, VacancyResponseDto>();
    }
}
