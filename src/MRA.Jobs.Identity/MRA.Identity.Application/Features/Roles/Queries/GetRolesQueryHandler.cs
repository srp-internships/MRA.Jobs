using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using MRA.Identity.Application.Contract.ApplicationRoles.Responses;
using MRA.Identity.Application.Contract;
using MRA.Identity.Application.Contract.ApplicationRoles.Queries;
using Microsoft.AspNetCore.Identity;
using MRA.Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace MRA.Identity.Application.Features.Roles.Queries;
public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, ApplicationResponse<List<RoleNameResponse>>>
{
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IMapper _mapper;

    public GetRolesQueryHandler(RoleManager<ApplicationRole> roleManager, IMapper mapper)
    {
        _roleManager = roleManager;
        _mapper = mapper;
    }

    public async Task<ApplicationResponse<List<RoleNameResponse>>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = await _roleManager.Roles.ToListAsync();

        var roleResponses = _mapper.Map<List<RoleNameResponse>>(roles);

        var response = new ApplicationResponseBuilder<List<RoleNameResponse>>()
            .SetResponse(roleResponses)
            .Success(true)
            .Build();

        return response;

    }

}

