using System.ComponentModel.DataAnnotations;

namespace FRUITABLE.ViewModels.ContentService
{
    public class ContentServiceEdit
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(25, ErrorMessage = "Max Length is 25")]
        public string? Name { get; set; }
        [Required]
        public string? Description { get; set; }
        public string? Image { get; set; }
        public IFormFile? Photo { get; set; }
    }
}
