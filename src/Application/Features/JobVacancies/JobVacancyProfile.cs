﻿using MRA.Jobs.Application.Contracts.JobVacancies.Commands;
using MRA.Jobs.Application.Contracts.JobVacancies.Responses;
using MRA.Jobs.Application.Contracts.TagDTO;
using MRA.Jobs.Application.Contracts.TimeLineDTO;

namespace MRA.Jobs.Application.Features.JobVacancies;

public class JobVacancyProfile : Profile
{
    public JobVacancyProfile()
    {
        CreateMap<JobVacancy, JobVacancyListDto>()
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name));
        CreateMap<JobVacancy, JobVacancyDetailsDto>()
            .ForMember(dest => dest.History, opt => opt.MapFrom(src => src.History))
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.Select(t => t.Tag)));
        CreateMap<CreateJobVacancyCommand, JobVacancy>();
        CreateMap<UpdateJobVacancyCommand, JobVacancy>();
        CreateMap<DeleteJobVacancyCommand, JobVacancy>();
        MappingConfiguration.ConfigureVacancyMap<VacancyTimelineEvent, TimeLineDetailsDto>(this);
        MappingConfiguration.ConfigureVacancyMap<Tag, TagDto>(this);
    }
}