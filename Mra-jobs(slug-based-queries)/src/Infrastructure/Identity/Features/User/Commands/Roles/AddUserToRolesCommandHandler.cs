﻿using Microsoft.AspNetCore.Identity;
using MRA.Jobs.Application.Common.Exceptions;
using MRA.Jobs.Infrastructure.Identity.Entities;
using MRA.Jobs.Infrastructure.Shared.Users.Commands.Roles;

namespace MRA.Jobs.Infrastructure.Identity.Features.User.Commands.Roles;

public class AddUserToRolesCommandHandler : IRequestHandler<AddUserToRolesCommand, Unit>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public AddUserToRolesCommandHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Unit> Handle(AddUserToRolesCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.Id.ToString());
        if (user == null)
            throw new NotFoundException(nameof(ApplicationUser), request.Id);

        var result = await _userManager.AddToRolesAsync(user, request.Roles);
        if (!result.Succeeded)
            throw new ValidationException(string.Join('\n', result.Errors.Select(r => r.Description)));

        return Unit.Value;
    }
}
