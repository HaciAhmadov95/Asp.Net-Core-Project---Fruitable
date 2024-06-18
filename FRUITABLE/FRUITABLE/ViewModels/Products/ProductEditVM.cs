using System.ComponentModel.DataAnnotations.Schema;

namespace FRUITABLE.ViewModels.Products
{
    public class ProductEditVM
    {
        public string Name { get; set; }



        public int? CategoryId { get; set; }


        [Column(TypeName = "decimal(18, 6)")]
        public decimal MinWeight { get; set; }

        public decimal Weight { get; set; }

        public string Origin { get; set; }

        public string? Quality { get; set; }

        public string Check { get; set; }

        public List<ProductImageVM>? Images { get; set; }
        public List<IFormFile>? Photos { get; set; }
    }
}
