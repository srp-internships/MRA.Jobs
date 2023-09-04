using Microsoft.AspNetCore.Identity;

namespace MRA.Identity.Domain.Entities;

public class ApplicationUser : IdentityUser<Guid>
{
    public List<ApplicationUserRole> UserRoles { get; set; }
}