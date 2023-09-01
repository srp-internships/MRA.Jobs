using MRA.Jobs.Application.Contracts.InternshipVacancies.Commands;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Responses;
using MRA.Jobs.Application.Contracts.TagDTO;
using MRA.Jobs.Application.Contracts.TimeLineDTO;

namespace MRA.Jobs.Application.Features.InternshipVacancies;

public class InternshipProfile : Profile
{
    public InternshipProfile()
    {
        CreateMap<InternshipVacancy, InternshipVacancyListResponse>()
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.VacancyQuestions, opt => opt.MapFrom(src => src.VacancyQuestions));
        CreateMap<InternshipVacancy, InternshipVacancyResponse>()
            .ForMember(dest => dest.History, opt => opt.MapFrom(src => src.History))
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.Select(t => t.Tag)));
        CreateMap<CreateInternshipVacancyCommand, InternshipVacancy>()
            .ForMember(dest => dest.VacancyQuestions, opt => opt.MapFrom(src => src.VacancyQuestions));
        CreateMap<UpdateInternshipVacancyCommand, InternshipVacancy>();
        MappingConfiguration.ConfigureVacancyMap<VacancyTimelineEvent, TimeLineDetailsDto>(this);
        MappingConfiguration.ConfigureVacancyMap<Tag, TagDto>(this);
    }
}