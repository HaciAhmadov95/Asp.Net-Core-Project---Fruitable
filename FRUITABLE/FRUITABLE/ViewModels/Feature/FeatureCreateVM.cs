using System.ComponentModel.DataAnnotations;

namespace FRUITABLE.ViewModels.Feature
{
    public class FeatureCreateVM
    {
        [Required]
        public string Icon { get; set; }

        [Required]
        [MaxLength(25, ErrorMessage = "Max Length is 25")]
        public string Content { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Max Length is 50")]
        public string Description { get; set; }
    }
}

