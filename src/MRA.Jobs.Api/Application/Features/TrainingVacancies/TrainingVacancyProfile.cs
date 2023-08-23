using MRA.Jobs.Application.Contracts.TagDTO;
using MRA.Jobs.Application.Contracts.TimeLineDTO;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Commands;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;

namespace MRA.Jobs.Application.Features.TrainingVacancies;

public class TrainingVacancyProfile : Profile
{
    public TrainingVacancyProfile()
    {
        CreateMap<TrainingVacancy, TrainingVacancyListDto>()
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name));
        CreateMap<TrainingVacancy, TrainingVacancyDetailedResponse>()
            .ForMember(dest => dest.History, opt => opt.MapFrom(src => src.History))
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.Select(t => t.Tag)));
        CreateMap<CreateTrainingVacancyCommand, TrainingVacancy>();
        CreateMap<UpdateTrainingVacancyCommand, TrainingVacancy>();
        MappingConfiguration.ConfigureVacancyMap<VacancyTimelineEvent, TimeLineDetailsDto>(this);
        MappingConfiguration.ConfigureVacancyMap<Tag, TagDto>(this);
    }
}