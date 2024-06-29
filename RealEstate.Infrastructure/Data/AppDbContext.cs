using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Entities.Property;
using RealEstate.Domain.Entities.Property.Property;

namespace RealEstate.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<PropertyType> PropertyTypes { get; set; }
        public DbSet<ListingType> ListingTypes { get; set; }

        public DbSet<FurnishingType> FurnishingTypes { get; set; }
        public DbSet<Amenity> Amenities { get; set; }
        public DbSet<Facility> Facilities { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
          
            modelBuilder.Entity<ListingType>().HasData(
            new ListingType { Id = 1, Name = "Buy" },
            new ListingType { Id = 2, Name = "Rent" },
            new ListingType { Id = 3, Name = "Commercial" }
        );


        }


    }
}
