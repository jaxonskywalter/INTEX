using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace INTEX.Data
{
    public class DataInitializer
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var logger = serviceProvider.GetRequiredService<ILogger<DataInitializer>>();

            string[] roleNames = { "Admin", "Researcher", "User" };

            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    var roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                    if (!roleResult.Succeeded)
                    {
                        logger.LogError("Failed to create role {RoleName}", roleName);
                    }
                }
            }

            var adminUser = new IdentityUser
            {
                UserName = "admin",
                Email = "admin@admin.com",
                EmailConfirmed = true
            };
            string adminPassword = "A3b!q7zP*8t2uYx";

            var user = await userManager.FindByEmailAsync(adminUser.Email);

            if (user == null)
            {
                var createResult = await userManager.CreateAsync(adminUser, adminPassword);
                if (createResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
                else
                {
                    logger.LogError("Failed to create admin user");
                }
            }
        }
    }
}
