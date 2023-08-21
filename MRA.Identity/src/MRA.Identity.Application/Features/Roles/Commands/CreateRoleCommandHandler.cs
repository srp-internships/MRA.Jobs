using MediatR;
using Microsoft.AspNetCore.Identity;
using MRA.Identity.Application.Contract;
using MRA.Identity.Application.Contract.ApplicationRoles.Commands;
using MRA.Identity.Domain.Entities;

namespace MRA.Identity.Application.Features.Roles.Commands;
public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, ApplicationResponse<Guid>>
{
    private readonly RoleManager<ApplicationRole> _roleManager;
    public CreateRoleCommandHandler(RoleManager<ApplicationRole> roleManager)
    {
        _roleManager = roleManager;
    }
    public async Task<ApplicationResponse<Guid>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var role = new ApplicationRole()
            {
                Id = Guid.NewGuid(),
                Name = request.RoleName,
                NormalizedName = request.RoleName.ToLower()
            };

            var result = await _roleManager.CreateAsync(role);

            if (result.Succeeded)
            {
                return new ApplicationResponseBuilder<Guid>().SetResponse(role.Id).Build();
            }

            return new ApplicationResponseBuilder<Guid>().SetErrorMessage(result.Errors.First().Description).Success(false).Build();
        }
        catch (Exception e)
        {
            return new ApplicationResponseBuilder<Guid>().Success(false).SetException(e).Build();
        }
    }
}
