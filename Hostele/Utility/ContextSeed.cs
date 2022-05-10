using Hostele.Models;
using Microsoft.AspNetCore.Identity;

namespace Hostele.Utility;

public class ContextSeed
{
    public static async Task SeedRolesAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        //Seed Roles
        await roleManager.CreateAsync(new IdentityRole(SD.Role_User));
        await roleManager.CreateAsync(new IdentityRole(SD.Role_Admin));
        /*await roleManager.CreateAsync(new IdentityRole(Enums.Roles.Moderator.ToString()));
        await roleManager.CreateAsync(new IdentityRole(Enums.Roles.Basic.ToString()));*/
    }
}