using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MRA.Identity.Domain.Entities;

namespace MRA.Identity.Infrastructure.Persistence;
public class ApplicationDbContextInitializer
{
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;
    public ApplicationDbContextInitializer(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }


    public async Task SeedAsync()
    {
        var role = new ApplicationRole
        {
            Id = Guid.NewGuid(),
            Name = "Name",
            NormalizedName = "name"

        };

        var roleExist = await _roleManager.RoleExistsAsync(role.Name);
        if (!roleExist)
        {
            await _roleManager.CreateAsync(role);
        }

        var user = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            UserName = "Name",
            NormalizedUserName = "name",
            Email = "Email",
            NormalizedEmail = "email",
            EmailConfirmed = false,
            PhoneNumber = "phoneNumber",

        };

        await _userManager.CreateAsync(user,"123test");
        await _userManager.AddToRoleAsync(user, role.Name);

    }
}

