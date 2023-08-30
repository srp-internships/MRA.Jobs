using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace MRA.Identity.Domain.Entities;
public class ApplicationUserRoles : IdentityUserRole<Guid>
{
    public Guid RoleId { get; set; }
    public ApplicationRole Role { get; set; }

    public Guid UserId {get; set; }
    public ApplicationUser User { get; set; }

    public string Slug { get; set; }
}
