

using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Data
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();

            string[] roles = new[] { "Admin", "Citizen", "Agency" };

            foreach (var role in roles)
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole<Guid>(role));

            if (!context.GovernmentEntities.Any())
            {
                var entities = new List<GovernmentEntity>
            {
                new() { Name = "Ministry of Health" },
                new() { Name = "Ministry of Education" },
                new() { Name = "Ministry of Interior" }
            };

                context.GovernmentEntities.AddRange(entities);
                await context.SaveChangesAsync();
            }

            var govEntities = context.GovernmentEntities.ToList();

            var adminEmail = config["AdminUser:Email"];
            var adminPassword = config["AdminUser:Password"];

            var admin = await userManager.FindByEmailAsync(adminEmail);
            if (admin == null)
            {
                admin = new ApplicationUser
                {
                    FullName = "System Administrator",
                    Email = adminEmail,
                    UserName = adminEmail,
                    IsVerified = true,
                    IsActive = true
                };

                await userManager.CreateAsync(admin, adminPassword);
                await userManager.AddToRoleAsync(admin, "Admin");
            }

            if (await userManager.FindByEmailAsync("citizen@gov.com") == null)
            {
                var citizen = new ApplicationUser
                {
                    FullName = "Test Citizen",
                    Email = "citizen@gov.com",
                    UserName = "citizen@gov.com",
                    IsVerified = true,
                    IsActive = true
                };

                await userManager.CreateAsync(citizen, "Citizen123*");
                await userManager.AddToRoleAsync(citizen, "Citizen");
            }

            foreach (var entity in govEntities)
            {
                var email = $"{entity.Name.Replace(" ", "").ToLower()}@gov.com";

                if (await userManager.FindByEmailAsync(email) == null)
                {
                    var employee = new ApplicationUser
                    {
                        FullName = $"Employee of {entity.Name}",
                        Email = email,
                        UserName = email,
                        GovernmentEntityId = entity.Id,
                        IsVerified = true,
                        IsActive = true
                    };

                    await userManager.CreateAsync(employee, "Agency123*");
                    await userManager.AddToRoleAsync(employee, "Agency");
                }
            }
        }
    }

}
