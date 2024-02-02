using MRA.Jobs.Application.Contracts.Dtos;

namespace MRA.Jobs.Application.Features.Vacancies.Questions;
public class VacancyQuestionProfile : Profile
{
    public VacancyQuestionProfile()
    {
        CreateMap<VacancyQuestionDto, VacancyQuestion>();
        CreateMap<VacancyQuestion, VacancyQuestionDto>();
        // CreateMap<VacancyQuestionDto, VacancyQuestion>();
        // CreateMap<VacancyQuestion, VacancyQuestionResponseDto>();
        // CreateMap<VacancyQuestionDto, VacancyQuestionResponseDto>();
    }
}
