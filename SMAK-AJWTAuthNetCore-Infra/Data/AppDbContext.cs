using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SMAK_AJWTAuthNetCore_Core.Entities;

namespace SMAK_AJWTAuthNetCore_Infra.Data
{
    public class AppDbContext : IdentityDbContext<RegisterRequestModel>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<RegisterRequestModel> ApplicationUsers { get; set; }

        public DbSet<LoginRequestModel>? LoggesInUsers { get; set; }

    }
}
