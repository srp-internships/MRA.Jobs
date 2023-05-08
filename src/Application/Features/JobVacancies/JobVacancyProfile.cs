using AutoMapper;
using MRA.Jobs.Application.Contracts.JobVacancies.Commands;
using MRA.Jobs.Application.Contracts.JobVacancies.Responses;
using MRA.Jobs.Domain.Entities;

namespace MRA.Jobs.Application.Features.JobVacancies;
public class JobVacancyProfile : Profile
{
    public JobVacancyProfile()
    {
        CreateMap<CreateJobVacancyCommand, JobVacancy>();
        CreateMap<UpdateJobVacancyCommand, JobVacancy>()
            .ForMember(e => e.Id, s => s.Ignore());

        CreateMap<List<JobVacancy>, List<JobVacancyResponse>>();
        CreateMap<JobVacancy, JobVacancyFullResponse>();
    }
}
