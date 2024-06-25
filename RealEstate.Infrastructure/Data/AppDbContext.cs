using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RealEstate.Domain.Entities;

namespace RealEstate.Infrastructure.Data
{
    public class AppDbContext:IdentityDbContext<User>
    {

        public AppDbContext(DbContextOptions<AppDbContext> options):base (options) { }
       public DbSet<User> Users {  get; set; }

    }
}
