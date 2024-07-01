using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Entities.Property;

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
            // Seed PropertyTypes
            modelBuilder.Entity<PropertyType>().HasData(
                new PropertyType { Id = 1, Name = "Apartment" },
                new PropertyType { Id = 2, Name = "House" },
                new PropertyType { Id = 3, Name = "Townhouse" },
                 new PropertyType { Id = 4, Name = "Penthouse" },
                new PropertyType { Id = 5, Name = "Compound" },
                new PropertyType { Id = 6, Name = "Duplex" },
                new PropertyType { Id = 7, Name = "Full Floor" },
                new PropertyType { Id = 8, Name = "Half Floor" },
                new PropertyType { Id = 9, Name = "Whole Building" },
                 new PropertyType { Id = 10, Name = "Bulk Rent Unit" },
                new PropertyType { Id = 11, Name = "Bungalow" },
                new PropertyType { Id = 12, Name = "Hotel & Apartment" }

            );

            // Seed FurnishingTypes
            modelBuilder.Entity<FurnishingType>().HasData(
                new FurnishingType { Id = 1, Name = "Furnished" },
                new FurnishingType { Id = 2, Name = "Unfurnished" },
                new FurnishingType { Id = 3, Name = "Partly Furnished" }
            );


            modelBuilder.Entity<ListingType>().HasData(
             new ListingType { Id = 1, Name = "Rent" },
             new ListingType { Id = 2, Name = "Buy" },
             new ListingType { Id = 3, Name = "Commercial" }
         );

            modelBuilder.Entity<Amenity>().HasData(
              new Amenity { Id = 1, Name = "Central A/C" },
              new Amenity { Id = 2, Name = "Balcony" },
              new Amenity { Id = 3, Name = "Shared Spa" },
              new Amenity { Id = 4, Name = "Concierge Service" },
              new Amenity { Id = 5, Name = "View of Water" },
              new Amenity { Id = 6, Name = "Pets Allowed" },
              new Amenity { Id = 7, Name = "Private Garden" },
              new Amenity { Id = 8, Name = "Private Gym" },
              new Amenity { Id = 9, Name = "Built in Wardrobes" },
              new Amenity { Id = 10, Name = "Built in Kitchen Appliances" },
              new Amenity { Id = 11, Name = "Maids Room" },
              new Amenity { Id = 12, Name = "Shared Pool" },
              new Amenity { Id = 13, Name = "Shared Gym" },
              new Amenity { Id = 14, Name = "Covered Parking" },
              new Amenity { Id = 15, Name = "View of Landmark" },
              new Amenity { Id = 16, Name = "Study" },
              new Amenity { Id = 17, Name = "Private Pool" },
              new Amenity { Id = 18, Name = "Private Jacuzzi" },
              new Amenity { Id = 19, Name = "Walk-in Closet" },
              new Amenity { Id = 20, Name = "Maid Service" },
                new Amenity { Id = 21, Name = "Children's Play Area" },
              new Amenity { Id = 22, Name = "Children's Pool" },
              new Amenity { Id = 23, Name = "Barbecue Area" }
          );


        }


    }
}
