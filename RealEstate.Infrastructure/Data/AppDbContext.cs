using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Entities.AgentEntity;
using RealEstate.Domain.Entities.CompanyEntity;
using RealEstate.Domain.Entities.PropertyEntity;

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

        public DbSet<Company> companies {  get; set; }
        public DbSet<CompanyStructure> companyStructures { get; set; }
        public DbSet<BusinessActivityType> businessActivityTypes { get; set; }

        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Plan> Plan { get; set; }

        public DbSet<Agent> Agents { get; set; }
        public DbSet<AgentImage> AgentImage { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
            modelBuilder.Entity<Company>()
       .HasOne(c => c.CompanyLogo)
       .WithOne(cf => cf.Company)
       .HasForeignKey<CompanyFile>(cf => cf.CompanyId)
       .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Notification>()
           .HasOne(n => n.User)
           .WithMany(u => u.Notifications)
           .HasForeignKey(n => n.UserId);

            base.OnModelCreating(modelBuilder);

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
                new Amenity { Id = 11, Name = "Children's Play Area" },
                new Amenity { Id = 12, Name = "Maids Room" },
                new Amenity { Id = 13, Name = "Shared Pool" },
                new Amenity { Id = 14, Name = "Shared Gym" },
                new Amenity { Id = 15, Name = "Covered Parking" },
                new Amenity { Id = 16, Name = "View of Landmark" },
                new Amenity { Id = 17, Name = "Study" },
                new Amenity { Id = 18, Name = "Private Pool" },
                new Amenity { Id = 19, Name = "Private Jacuzzi" },
                new Amenity { Id = 20, Name = "Walk-in Closet" },
                new Amenity { Id = 21, Name = "Maid Service" },
                new Amenity { Id = 22, Name = "Children's Pool" },
                new Amenity { Id = 23, Name = "Barbecue Area" }

            );

            // Seed data for BusinessActivityTypes
            modelBuilder.Entity<BusinessActivityType>().HasData(
                new BusinessActivityType { Id = 1, Name = "Real Estate Brokerage" },
                new BusinessActivityType { Id = 2, Name = "Property Management" }
            );

            // Seed data for CompanyStructures
            modelBuilder.Entity<CompanyStructure>().HasData(
                new CompanyStructure { Id = 1, Name = "Sole Proprietorship" },
                new CompanyStructure { Id = 2, Name = "Limited Liability Company" },
                 new CompanyStructure { Id = 3, Name = "Civil Company" },
                new CompanyStructure { Id = 4, Name = "Free Zone Establishment)" }
            );

            // Seed data for Facilities
            modelBuilder.Entity<Facility>().HasData(
                new Facility { Id = 1, Name = "School" },
                new Facility { Id = 2, Name = "Hospital" },
                new Facility { Id = 3, Name = "Public Transport" },
                new Facility { Id = 4, Name = "Shopping Mall" },
                new Facility { Id = 5, Name = "Park" },
                new Facility { Id = 6, Name = "Metro" }
            );

            // Seed data for FurnishingTypes
            modelBuilder.Entity<FurnishingType>().HasData(
                new FurnishingType { Id = 1, Name = "Furnished" },
                new FurnishingType { Id = 2, Name = "Unfurnished" },
               new FurnishingType { Id = 3, Name = "Partly Furnished" }

            );

            // Seed data for ListingTypes
            modelBuilder.Entity<ListingType>().HasData(
                new ListingType { Id = 1, Name = "Rent" },
                new ListingType { Id = 2, Name = "Buy" },
                new ListingType { Id = 3, Name = "Commercial" }

            );
            modelBuilder.Entity<Plan>().HasData(
                new Plan { Id = "p1", Name = "Free Trial", Price =0 },
                new Plan { Id = "price_1PZ4m0GFthNCZxNOoti2pHeh", Name = "Basic" ,Price=350},
                new Plan { Id = "price_1PZ4mUGFthNCZxNOwjVLIVd6", Name = "Premium",Price=700 }

            );

        }


    }
}
