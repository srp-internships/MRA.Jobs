using Microsoft.AspNetCore.Identity;
using MRA.Identity.Domain.Enumes;

namespace MRA.Identity.Domain.Entities;

public class ApplicationUser : IdentityUser<Guid>
{
    public Gender Gender { get; set; }
    public DateTime DateOfBirth { get; set; }
    public List<ApplicationUserRole> UserRoles { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}