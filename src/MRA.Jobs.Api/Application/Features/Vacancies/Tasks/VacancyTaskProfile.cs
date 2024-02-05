using MRA.Jobs.Application.Contracts.Dtos;
using MRA.Jobs.Application.Contracts.Dtos.Responses;

namespace MRA.Jobs.Application.Features.Vacancies.Tasks;
public class VacancyTaskProfile : Profile
{
    public VacancyTaskProfile()
    {
        CreateMap<VacancyTaskDto,VacancyTask>();
        CreateMap<VacancyTask, VacancyTaskDto>();
        CreateMap<VacancyTaskResponseDto, VacancyTask>();
        CreateMap<VacancyTask, VacancyTaskResponseDto>();
        CreateMap<VacancyTaskDto, VacancyTaskResponseDto>();

    }
}
