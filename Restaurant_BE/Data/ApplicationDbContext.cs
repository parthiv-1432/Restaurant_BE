using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Restaurant_BE.Model;

namespace Restaurant_BE.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // 🔹 Seed Static Roles with GUIDs
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Administrator",
                    NormalizedName = "ADMINISTRATOR",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                },
                new IdentityRole
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Steward",
                    NormalizedName = "STEWARD",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                },
                new IdentityRole
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Biller",
                    NormalizedName = "BILLER",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                },
                new IdentityRole
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Chef",
                    NormalizedName = "CHEF",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                }
            );
        }
    }
}
