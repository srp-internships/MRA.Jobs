using MRA.Jobs.Application.Contracts.TimeLineDTO;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Commands.Create;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Commands.Update;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;
using MRA.Jobs.Application.Contracts.VacancyCategories.Responses;

namespace MRA.Jobs.Application.Features.TrainingVacancies;

public class TrainingVacancyProfile : Profile
{
    public TrainingVacancyProfile()
    {
        CreateMap<TrainingVacancy, TrainingVacancyListDto>()
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name));
        CreateMap<TrainingVacancy, TrainingVacancyDetailedResponse>()
            .ForMember(dest => dest.History, opt => opt.MapFrom(src => src.History))
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.Select(t => t.Tag.Name)));
        CreateMap<CreateTrainingVacancyCommand, TrainingVacancy>()
            .ForMember(dest => dest.VacancyQuestions, opt => opt.MapFrom(src => src.VacancyQuestions))
            .ForMember(dest => dest.VacancyTasks, opt => opt.MapFrom(src => src.VacancyTasks));
        CreateMap<UpdateTrainingVacancyCommand, TrainingVacancy>();
        CreateMap<CategoryResponse, VacancyCategory>();
        CreateMap<TrainingVacancyListDto, TrainingVacancy>();
        MappingConfiguration.ConfigureVacancyMap<VacancyTimelineEvent, TimeLineDetailsDto>(this);
    }
}