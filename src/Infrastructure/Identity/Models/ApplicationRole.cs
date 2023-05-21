using Microsoft.AspNetCore.Identity;

namespace MRA.Jobs.Infrastructure.Identity.Models;

public class ApplicationRole : IdentityRole<Guid>
{
    public ApplicationRole() : base() { }

    public ApplicationRole(string name) : base(name) { }
}