using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Entities.Company;
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

        public DbSet<Company> companies {  get; set; }
        public DbSet<CompanyStructure> companyStructures { get; set; }
        public DbSet<BusinessActivityType> businessActivityTypes { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            // Seed PropertyTypes
            modelBuilder.Entity<CompanyStructure>().HasData(
                new CompanyStructure { Id = 1, Name = "Sole Proprietorship" },
                new CompanyStructure { Id = 2, Name = "Limited Liability Company" },
                new CompanyStructure { Id = 3, Name = "Civil Company" },
                 new CompanyStructure { Id = 4, Name = "Free Zone Establishment" }
            );
            modelBuilder.Entity<BusinessActivityType>().HasData(
              new BusinessActivityType { Id = 1, Name = "Property management" },
              new BusinessActivityType { Id = 2, Name = "Real Estate Brokerage" }
          );

        }


    }
}
