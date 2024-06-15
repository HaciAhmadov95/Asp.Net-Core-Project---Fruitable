
using FRUITABLE.Data;
using FRUITABLE.Models;
using FRUITABLE.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FRUITABLE.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<Slider> sliders = await _context.Slider.ToListAsync();
            SliderInfo sliderInfo = await _context.SliderInfo.FirstOrDefaultAsync();
            List<Category> categories = await _context.Categories.ToListAsync();
            List<Features> features = await _context.Features.ToListAsync();
            List<Product> products = await _context.Products.Include(m => m.ProductImages).ToListAsync();
            List<FactFeatureContent> factFeatureContents = await _context.factFeatureContents.ToListAsync();

            HomeVM model = new()
            {
                Sliders = sliders,
                SliderInfos = sliderInfo,
                Categories = categories,
                Features = features,
                Products = products,
                FactFeatureContents = factFeatureContents

            };

            return View(model);
        }
    }
}