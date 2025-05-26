using Microsoft.EntityFrameworkCore;
using TeamTrackPro.API.Models;

namespace TeamTrackPro.API.Data.Seed
{
    public class UserSeeder : BaseSeeder<User>
    {
        public UserSeeder(ModelBuilder modelBuilder) : base(modelBuilder)
        {
        }

        public override void Seed()
        {
            var adminUser = new User
            {
                Id = 1,
                Username = "admin",
                Email = "admin@teamtrackpro.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                FirstName = "System",
                LastName = "Administrator",
                RoleId = 1
            };

            SetCommonProperties(adminUser);

            _modelBuilder.Entity<User>().HasData(adminUser);
        }
    }
} 