using FRUITABLE.Models;

namespace FRUITABLE.ViewModels
{
    public class ShopVM
    {
        public Product Product { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public List<Category> Categories { get; set; }
    }
}
