using MRA.Jobs.Application.Contracts.Dtos;

namespace MRA.Jobs.Application.Features.VacancyQuestions;
public class VacancyQuestionProfile : Profile
{
    public VacancyQuestionProfile()
    {
        CreateMap<VacancyQuestionDto, VacancyQuestion>();
    }
}
