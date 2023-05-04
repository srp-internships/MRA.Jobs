using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MRA.Jobs.Application.Contracts.JobVacancies.Commands;
using MRA.Jobs.Domain.Entities;

namespace MRA.Jobs.Application.Features.JobVacancies;
internal class JobVacancyProfile : Profile
{
    public JobVacancyProfile()
    {
        CreateMap<CreateJobVacancyCommand, JobVacancy>();
        CreateMap<UpdateJobVacancyCommand, JobVacancy>()
            .ForMember(e => e.Id, s => s.Ignore());


    }
}
