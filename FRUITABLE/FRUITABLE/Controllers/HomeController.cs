
using FRUITABLE.Data;
using FRUITABLE.Models;
using FRUITABLE.Services.Interface;
using FRUITABLE.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FRUITABLE.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IProductService _productService;
        private readonly ICommentService _commentService;

        public HomeController(AppDbContext context, IProductService productService, ICommentService commentService)
        {
            _context = context;
            _productService = productService;
            _commentService = commentService;
        }

        public async Task<IActionResult> Index()
        {
            List<Slider> sliders = await _context.Slider.ToListAsync();
            SliderInfo sliderInfo = await _context.SliderInfo.FirstOrDefaultAsync();
            List<Category> categories = await _context.Categories.ToListAsync();
            List<Features> features = await _context.Features.ToListAsync();
            List<Product> products = await _productService.GetAllAsync();
            List<FactFeatureContent> factFeatureContents = await _context.factFeatureContents.ToListAsync();
            List<ContentService> contentServices = await _context.contentServices.ToListAsync();
            FreshContent freshContent = await _context.freshContents.FirstOrDefaultAsync();
            List<Comments> comments = await _context.Comments.ToListAsync();

            HomeVM model = new()
            {
                Sliders = sliders,
                SliderInfos = sliderInfo,
                Categories = categories,
                Features = features,
                Products = products,
                FactFeatureContents = factFeatureContents,
                contentServices = contentServices,
                FreshContent = freshContent,
                Comments = comments,

            };

            return View(model);
        }
    }
}