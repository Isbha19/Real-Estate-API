using RealEstate.Domain.Entities.AgentEntity;
using RealEstate.Domain.Entities.CompanyEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstate.Domain.Entities.PropertyEntity
{
    public class Property : baseEntity
    {
        [Required]
        [StringLength(100)]
        public string PropertyTitle { get; set; }

        [Required]
        [StringLength(600)]
        public string PropertyDescription { get; set; }

        [Required]
        public int PropertyTypeId { get; set; }
        public  PropertyType PropertyType { get; set; }

        [Required]
        public int ListingTypeId { get; set; }
        public  ListingType ListingType { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Price { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Bedrooms { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Bathrooms { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Size { get; set; }

        public int FurnishingTypeId { get; set; }
        public  FurnishingType FurnishingType { get; set; }

        public  ICollection<PropertyAmenties> PropertyAmenties { get; set; }
        public  ICollection<PropertyNearByFacilities> PropertyNearByFacilities { get; set; }
        public  ICollection<Image> Images { get; set; }

        public string VirtualTourUrl { get; set; }
        public string VideoTourUrl { get; set; }
        public bool IsCompanyAdminVerified { get; set; }
        public DateTime PostedOn { get; set; } = DateTime.Now;
        public int PropertyViews { get; set; } = 0;


        [ForeignKey("Agent")]
        public int AgentId { get; set; }  // Foreign key to Agent entity

        public  Agent Agent { get; set; }  // Navigation property to Agent

    
        public DateOnly AvailabilityDate { get; set; }
    }
}
