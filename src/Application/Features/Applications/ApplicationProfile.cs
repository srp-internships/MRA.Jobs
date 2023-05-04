using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MRA.Jobs.Application.Contracts.Applications.Commands;

namespace MRA.Jobs.Application.Features.Applications;

using MRA.Jobs.Application.Features.Applications.Query;
using MRA.Jobs.Domain.Entities;
public class ApplicationProfile: Profile
{
    public ApplicationProfile()
    {
        CreateMap<CreateApplicationCommand, Application>();
        CreateMap<GetApplicationsQuery, Application>();
    }
}
