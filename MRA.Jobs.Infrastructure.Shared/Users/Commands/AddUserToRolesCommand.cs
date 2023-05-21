﻿using MediatR;

namespace MRA.Jobs.Infrastructure.Shared.Users.Commands;

public class AddUserToRolesCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
    public IEnumerable<string> Roles { get; set; }
}
