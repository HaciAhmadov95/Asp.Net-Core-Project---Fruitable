using System.ComponentModel.DataAnnotations;

namespace FRUITABLE.ViewModels.ContentService
{
    public class ContentServiceCreate
    {

        [Required]
        [MaxLength(25, ErrorMessage = "Max Length is 25")]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public IFormFile? Image { get; set; }
    }
}
