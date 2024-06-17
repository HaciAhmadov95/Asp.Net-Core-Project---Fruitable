using System.ComponentModel.DataAnnotations;

namespace FRUITABLE.ViewModels.SliderInfos
{
    public class SliderInfoEditVM
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
    }
}
