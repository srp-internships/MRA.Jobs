using MRA.Jobs.Application.Contracts.Dtos;

namespace MRA.Jobs.Application.Features.VacancyTasks;
public class VacancyTaskProfile : Profile
{
    public VacancyTaskProfile()
    {
        CreateMap<VacancyTask, VacancyTaskDto>();
        CreateMap<VacancyTaskDto, VacancyTask>();
    }

}
