﻿using MRA.Jobs.Application.Contracts.InternshipVacancies.Commands.Create;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Commands.Update;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Responses;
using MRA.Jobs.Application.Contracts.TimeLineDTO;

namespace MRA.Jobs.Application.Features.InternshipVacancies;

public class InternshipProfile : Profile
{
    public InternshipProfile()
    {
        CreateMap<InternshipVacancy, InternshipVacancyListResponse>()
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.VacancyQuestions, opt => opt.MapFrom(src => src.VacancyQuestions))
            .ForMember(dest => dest.VacancyTasks, opt => opt.MapFrom(src => src.VacancyTasks))
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.Select(t=>t.Tag.Name)));
        CreateMap<InternshipVacancy, InternshipVacancyResponse>()
            .ForMember(dest => dest.History, opt => opt.MapFrom(src => src.History))
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.Select(t => t.Tag.Name)));
        CreateMap<CreateInternshipVacancyCommand, InternshipVacancy>()
            .ForMember(dest => dest.VacancyQuestions, opt => opt.MapFrom(src => src.VacancyQuestions))
            .ForMember(dest => dest.VacancyTasks, opt => opt.MapFrom(src => src.VacancyTasks));
        CreateMap<UpdateInternshipVacancyCommand, InternshipVacancy>();
        MappingConfiguration.ConfigureVacancyMap<VacancyTimelineEvent, TimeLineDetailsDto>(this);
    }
}