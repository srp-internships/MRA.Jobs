﻿using MRA.Jobs.Application.Contracts.Dtos;

namespace MRA.Jobs.Application.Features.Vacancies.Tasks;
public class VacancyTaskResponseProfile :Profile
{
    public VacancyTaskResponseProfile()
    {
        CreateMap<TaskResponseDto, TaskResponse>()
        .ForMember(dest => dest.TaksId, opt => opt.MapFrom(src => src.TaskId));
        CreateMap<TaskResponse, TaskResponseDto>();
    }
}
