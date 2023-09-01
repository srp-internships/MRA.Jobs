using Microsoft.AspNetCore.Identity;

namespace MRA.Identity.Domain.Entities;
public class ApplicationUserRole : IdentityUserRole<Guid>
{
    public string Slug { get; set; }
}
