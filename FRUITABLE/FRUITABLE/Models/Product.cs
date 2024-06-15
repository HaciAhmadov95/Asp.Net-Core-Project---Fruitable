using System.ComponentModel.DataAnnotations.Schema;

namespace FRUITABLE.Models
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public string Quality { get; set; }
        public string Check { get; set; }
        public string CountrOfOrigin { get; set; }
        [Column(TypeName = "decimal(18, 6)")]
        public decimal Weight { get; set; }
        [Column(TypeName = "decimal(18, 6)")]
        public decimal MinWeight { get; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<ProductImages> ProductImages { get; }






    }
}
