using FRUITABLE.Data;
using FRUITABLE.Models;
using FRUITABLE.ViewModels.SliderInfos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FRUITABLE.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderInfoController : Controller
    {
        private readonly AppDbContext _context;
        public SliderInfoController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<SliderInfo> sliderInfos = await _context.SliderInfo.ToListAsync();
            return View(sliderInfos);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();
            SliderInfo sliderInfo = await _context.SliderInfo.Where(c => c.Id == id).FirstOrDefaultAsync();
            if (sliderInfo == null) return NotFound();
            SliderInfoDetailVM model = new()
            {
                Id = sliderInfo.Id,
                Title = sliderInfo.Title,

            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SliderInfoCreateVM sliderInfo)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            bool existSliderInfo = await _context.SliderInfo.AnyAsync(m => m.Title == sliderInfo.Title);
            if (existSliderInfo)
            {
                ModelState.AddModelError("Title", "These inputs already exist");
            }

            await _context.SliderInfo.AddAsync(new SliderInfo { Title = sliderInfo.Title });
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }



        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            SliderInfo sliderInfo = await _context.SliderInfo.FirstOrDefaultAsync(m => m.Id == id);

            if (sliderInfo == null) return NotFound();

            _context.SliderInfo.Remove(sliderInfo);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (id == null) return BadRequest();
            SliderInfo sliderInfo = await _context.SliderInfo.FirstOrDefaultAsync(m => m.Id == id);

            if (sliderInfo == null) return NotFound();

            return View(new SliderInfoEditVM
            {
                Title = sliderInfo.Title,
            });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SliderInfoEditVM sliderInfo, int? id)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (id == null) return BadRequest();
            SliderInfo existSliderInfo = await _context.SliderInfo.FirstOrDefaultAsync(m => m.Id == id);

            if (sliderInfo == null) return NotFound();

            existSliderInfo.Title = sliderInfo.Title;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }
    }
}
