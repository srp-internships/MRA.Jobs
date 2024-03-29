﻿using MRA.Jobs.Application.Contracts.JobVacancies.Commands.CreateJobVacancy;
using MRA.Jobs.Application.Contracts.JobVacancies.Commands.DeleteJobVacancy;
using MRA.Jobs.Application.Contracts.JobVacancies.Commands.Update;
using MRA.Jobs.Application.Contracts.JobVacancies.Responses;
using MRA.Jobs.Application.Contracts.TimeLineDTO;

namespace MRA.Jobs.Application.Features.JobVacancies;

public class JobVacancyProfile : Profile
{
    public JobVacancyProfile()
    {
        CreateMap<JobVacancy, JobVacancyListDto>()
            .ForMember(dest => dest.Category,
                opt => opt.MapFrom(
                    src => src.Category.Name))
            .ForMember(dest => dest.Tags,
                opt => opt.MapFrom(
                    src => src.Tags.Select(t => t.Tag.Name)));

        CreateMap<JobVacancy, JobVacancyDetailsDto>()
            .ForMember(dest => dest.History,
                opt => opt.MapFrom(
                    src => src.History))
            .ForMember(dest => dest.Tags,
                opt => opt.MapFrom(
                    src => src.Tags.Select(t => t.Tag.Name)));

        CreateMap<CreateJobVacancyCommand, JobVacancy>()
            .ForMember(dest => dest.VacancyQuestions,
                opt => opt.MapFrom(
                    src => src.VacancyQuestions))
            .ForMember(dest => dest.VacancyTasks,
                opt => opt.MapFrom(
                    src => src.VacancyTasks));

        CreateMap<UpdateJobVacancyCommand, JobVacancy>();
        CreateMap<DeleteJobVacancyCommand, JobVacancy>();
        MappingConfiguration.ConfigureVacancyMap<VacancyTimelineEvent, TimeLineDetailsDto>(this);
    }
}