using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;


public class DataSeeder
{
    public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

        string[] roleNames = { "Admin", "Researcher", "User" };

        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        var adminUser = new IdentityUser
        {
            UserName = "ADMIN@ADMIN.COM",
            Email = "ADMIN@ADMIN.COM",
        };
        string adminPassword = "A3b!q7zP*8t2uYx";
        var users = await userManager.Users
                .Where(u => u.NormalizedEmail == adminUser.NormalizedEmail)
                .ToListAsync();

        var user = await userManager.FindByEmailAsync(adminUser.Email);

        if (user == null)
        {
            var createResult = await userManager.CreateAsync(adminUser, adminPassword);
            if (createResult.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");

                // Confirm the admin user's email
                var emailConfirmationToken = await userManager.GenerateEmailConfirmationTokenAsync(adminUser);
                await userManager.ConfirmEmailAsync(adminUser, emailConfirmationToken);
            }
        }
    }
}
