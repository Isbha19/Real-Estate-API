using RealEstate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace RealEstate.Application.DTOs.Request.Property
{
    public class propertyDto
    {


        [Required]
        [StringLength(100)]
        public string PropertyTitle { get; set; }
        [Required]
        [StringLength(600)]
        public string PropertyDescription { get; set; }
        [Required]

        public int PropertyTypeId { get; set; }
        [Required]

        public int ListingTypeId { get; set; }
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
        public List<IFormFile> Images { get; set; }
        public string PropertyAmenties { get; set; }
        public string PropertyNearByFacilities { get; set; }
        public string VirtualTourUrl { get; set; }
        public string VideoTourUrl { get; set; }


       public DateOnly AvailabilityDate { get; set; }

    }
}


