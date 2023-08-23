using Microsoft.AspNetCore.Identity;

namespace MRA.Identity.Domain.Entities;

public class ApplicationRole : IdentityRole<Guid>
{
    public string Slug { get; set; }
}