using FRUITABLE.Data;
using FRUITABLE.Helpers.Extensions;
using FRUITABLE.Models;
using FRUITABLE.ViewModels.Sliders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FRUITABLE.Areas.Admin.Controllers
{


    [Area("Admin")]
    public class SliderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public SliderController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<Slider> sliders = await _context.Slider.ToListAsync();
            return View(sliders);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SliderCreateVM create)
        {
            if (!ModelState.IsValid) return View();

            if (!create.Image.CheckFileType("image/"))
            {
                ModelState.AddModelError("Image", "File must be Image Format");
                return View();
            }
            if (!create.Image.CheckFileSize(200))
            {

                ModelState.AddModelError("Image", "Max File Capacity mut be 300KB");
                return View();
            }
            string fileName = Guid.NewGuid().ToString() + "-" + create.Image.FileName;
            string path = Path.Combine(_env.WebRootPath, "img", fileName);
            await create.Image.SaveFileToLocalAsync(path);
            await _context.Slider.AddAsync(new Slider { Image = fileName, SliderTitle = create.Name });
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();
            Slider slider = await _context.Slider.Where(c => c.Id == id).FirstOrDefaultAsync();
            if (slider == null) return NotFound();
            SliderDetailVM model = new()
            {
                Id = slider.Id,
                Name = slider.SliderTitle,
                Image = slider.Image

            };
            return View(model);
        }
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();

            Slider slide = await _context.Slider.FirstOrDefaultAsync(s => s.Id == id);

            if (slide is null) return NotFound();

            slide.Image.DeleteFile(_env.WebRootPath, "img");

            _context.Slider.Remove(slide);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));



        }
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null) return BadRequest();
            Slider slider = await _context.Slider.FirstOrDefaultAsync(m => m.Id == id);
            if (slider == null) return NotFound();

            return View(new SliderEditVM { Name = slider.SliderTitle, Image = slider.Image });


        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, SliderEditVM request)
        {
            Slider existSlider = await _context.Slider.FirstOrDefaultAsync(m => m.Id == id);
            if (!ModelState.IsValid)
            {
                request.Image = existSlider.Image;
                return View(request);
            }

            if (request.Photo != null)
            {
                if (!request.Photo.CheckFileSize(200))
                {
                    ModelState.AddModelError("Photo", "Image size must be 200kb");
                    return View(request.Photo);
                }

                if (!request.Photo.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Photo", "Image format is wrong");
                    return View(request.Photo);
                }

                FileExtensions.DeleteFileFromLocalAsync(Path.Combine(_env.WebRootPath, "img"), existSlider.Image);

                string fileName = Guid.NewGuid().ToString() + "-" + request.Photo.FileName;
                string path = Path.Combine(_env.WebRootPath, "img", fileName);
                await request.Photo.SaveFileToLocalAsync(path);

                existSlider.Image = fileName;
            }

            if (existSlider == null) { return NotFound(); }

            existSlider.SliderTitle = request.Name;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
    }
}
