using RealEstate.Domain.Entities.Property;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Domain.Entities.Property.Property
{
    public class Property:baseEntity
    {
        [Required]
        [StringLength(100)]
        public string PropertyTitle { get; set; }
        [Required]
        [StringLength(600)]
        public string PropertyDescription { get; set; }
        [Required]

        public int PropertyTypeId { get; set; }
        public PropertyType PropertyType { get; set; }
        [Required]

        public int ListingTypeId { get; set; }
        public ListingType ListingType { get; set; }
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
        public FurnishingType FurnishingType { get; set; }
        public ICollection<PropertyAmenties> PropertyAmenties { get; set; }
        public ICollection<PropertyNearByFacilities> PropertyNearByFacilities { get; set; }
        public ICollection<Image> Images { get; set; }
        public string VirtualTourUrl { get; set; }
        public string VideoTourUrl { get; set; }


        public DateTime PostedOn { get; set; } = DateTime.Now;
        [ForeignKey("User")]
        public string AgentId { get; set; }
        public User user { get; set; }
        public DateOnly AvailabilityDate { get; set; }

    }
}
