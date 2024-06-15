using System.ComponentModel.DataAnnotations;

namespace FRUITABLE.ViewModels.Categories
{
    public class CategoryCreateVM
    {

        [Required(ErrorMessage = "This field can't be empty")]
        [StringLength(20, ErrorMessage = "Max length must be 20")]
        public string Name { get; set; }
    }
}
