﻿using Microsoft.AspNetCore.Identity;

namespace MRA.Jobs.Infrastructure.Identity.Entities;

public class ApplicationRole : IdentityRole<Guid>
{
    public ApplicationRole()
    {
    }

    public ApplicationRole(string name) : base(name) { }
}