using MRA.Jobs.Application.Contracts.Dtos;

namespace MRA.Jobs.Application.Features.VacancyTasks;
public class VacancyTaskResponseProfile :Profile
{
    public VacancyTaskResponseProfile()
    {
        CreateMap<TaskResponseDto, TaskResponse>();
        CreateMap<TaskResponse, TaskResponseDto>();
    }
}
