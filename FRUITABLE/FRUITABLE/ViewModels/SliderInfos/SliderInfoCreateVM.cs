using System.ComponentModel.DataAnnotations;

namespace FRUITABLE.ViewModels.SliderInfos
{
    public class SliderInfoCreateVM
    {

        [Required]
        public string Title { get; set; }
    }
}
