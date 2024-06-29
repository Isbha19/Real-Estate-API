
using System.ComponentModel.DataAnnotations.Schema;


namespace RealEstate.Domain.Entities.Property.Property
{
    [Table("Images")]
    public class Image : baseEntity
    {
        public string ImageUrl { get; set; }
        public bool IsPrimary { get; set; }
        public int PropertId { get; set; }
        public Property Property { get; set; }
    }
}
