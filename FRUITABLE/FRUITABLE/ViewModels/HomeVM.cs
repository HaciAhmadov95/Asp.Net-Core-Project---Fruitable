using FRUITABLE.Models;

namespace FRUITABLE.ViewModels
{
    public class HomeVM
    {
        public List<Slider> Sliders { get; set; }
        public SliderInfo SliderInfos { get; set; }
        public List<Category> Categories { get; set; }
        public List<Features> Features { get; set; }
        public List<Product> Products { get; set; }
        public List<Models.FactFeatureContent> FactFeatureContents { get; set; }
        public List<Models.ContentService> contentServices { get; set; }
        public FreshContent FreshContent { get; set; }
    }
}
