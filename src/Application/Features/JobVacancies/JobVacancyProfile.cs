using MRA.Jobs.Application.Contracts.JobVacancies.Commands;
using MRA.Jobs.Application.Contracts.JobVacancies.Responses;

namespace MRA.Jobs.Application.Features.JobVacancies;
public class JobVacancyProfile : Profile
{
    public JobVacancyProfile()
    {
        CreateMap<JobVacancy, JobVacancyListDTO>();
        CreateMap<JobVacancy, JobVacancyDetailsDTO>();
        CreateMap<CreateJobVacancyCommand, JobVacancy>();
        CreateMap<UpdateJobVacancyCommand, JobVacancy>();
    }
}
