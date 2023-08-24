using Microsoft.AspNetCore.Identity;

namespace MRA.Identity.Domain.Entities;

public class ApplicationUserClaim:IdentityUserClaim<Guid>
{
    public string Slug { get; set; } = "";
}