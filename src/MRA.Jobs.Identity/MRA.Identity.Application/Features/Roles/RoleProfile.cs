using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MRA.Identity.Application.Contract.ApplicationRoles.Responses;
using MRA.Identity.Domain.Entities;

namespace MRA.Identity.Application.Features.Roles;
public class RoleProfile : Profile
{
    public RoleProfile()
    {
        CreateMap<ApplicationRole, RoleNameResponse>();
       
    }
}

