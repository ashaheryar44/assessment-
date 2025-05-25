using Microsoft.EntityFrameworkCore;
using TeamTrackPro.API.Helpers;
using TeamTrackPro.API.Models;

namespace TeamTrackPro.API.Data.Seed;

public static class DbSeeder
{
    public static async Task SeedRolesAsync(AppDbContext context)
    {
        if (await context.Roles.AnyAsync())
        {
            return;
        }

        var roles = new List<Role>
        {
            new()
            {
                Name = RoleConstants.Admin,
                Description = RoleConstants.RoleDescriptions[RoleConstants.Admin],
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            },
            new()
            {
                Name = RoleConstants.Manager,
                Description = RoleConstants.RoleDescriptions[RoleConstants.Manager],
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            },
            new()
            {
                Name = RoleConstants.User,
                Description = RoleConstants.RoleDescriptions[RoleConstants.User],
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            }
        };

        await context.Roles.AddRangeAsync(roles);
        await context.SaveChangesAsync();
    }

    public static async Task SeedAdminUserAsync(AppDbContext context)
    {
        if (await context.Users.AnyAsync(u => u.Username == "admin"))
        {
            return;
        }

        var adminRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == RoleConstants.Admin);
        if (adminRole == null)
        {
            throw new Exception("Admin role not found. Please run SeedRolesAsync first.");
        }

        var adminUser = new User
        {
            Username = "admin",
            Email = "admin@teamtrackpro.com",
            PasswordHash = "admin123", // TODO: Implement proper password hashing
            FirstName = "System",
            LastName = "Administrator",
            RoleId = adminRole.Id,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        await context.Users.AddAsync(adminUser);
        await context.SaveChangesAsync();
    }
} 