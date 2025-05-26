using Microsoft.EntityFrameworkCore;
using TeamTrackPro.API.Models;

namespace TeamTrackPro.API.Data.Seed
{
    public class RoleSeeder : BaseSeeder<Role>
    {
        public RoleSeeder(ModelBuilder modelBuilder) : base(modelBuilder)
        {
        }

        public override void Seed()
        {
            var roles = new[]
            {
                new Role
                {
                    Id = 1,
                    Name = "Admin",
                    Description = "Administrator with full access",
                    IsActive = true
                },
                new Role
                {
                    Id = 2,
                    Name = "Manager",
                    Description = "Project manager with elevated permissions",
                    IsActive = true
                },
                new Role
                {
                    Id = 3,
                    Name = "Developer",
                    Description = "Developer with access to assigned projects",
                    IsActive = true
                },
                new Role
                {
                    Id = 4,
                    Name = "Tester",
                    Description = "Tester with access to assigned tickets",
                    IsActive = true
                }
            };

            foreach (var role in roles)
            {
                SetCommonProperties(role);
            }

            _modelBuilder.Entity<Role>().HasData(roles);
        }
    }
} 