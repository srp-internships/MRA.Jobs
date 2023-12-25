using MRA.Jobs.Application.Contracts.Dtos;
using MRA.Jobs.Application.Contracts.Dtos.Responses;

namespace MRA.Jobs.Application.Features.VacancyQuestions;
public class VacancyQuestionProfile : Profile
{
    public VacancyQuestionProfile()
    {
        CreateMap<VacancyQuestionDto, VacancyQuestion>();
        CreateMap<VacancyQuestion, VacancyQuestionDto>();
        CreateMap<VacancyQuestionResponseDto, VacancyQuestion>();
        CreateMap<VacancyQuestion, VacancyQuestionResponseDto>();
        CreateMap<VacancyQuestionDto, VacancyQuestionResponseDto>();
    }
}
