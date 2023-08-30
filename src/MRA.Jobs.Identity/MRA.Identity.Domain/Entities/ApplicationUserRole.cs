using Microsoft.AspNetCore.Identity;

namespace MRA.Identity.Domain.Entities;
public class ApplicationUserRole : IdentityUserRole<Guid>
{
    public ApplicationUser User { get; set; }
    public Guid UserId { get; set; }

    public Guid RoleId { get; set; }
    public ApplicationRole Role { get; set; }
    public string Slug { get; set; }
}
