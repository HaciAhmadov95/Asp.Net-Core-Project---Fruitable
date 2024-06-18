using FRUITABLE.Data;
using FRUITABLE.Helpers.Extensions;
using FRUITABLE.Models;
using FRUITABLE.ViewModels.Fresh;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FRUITABLE.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class FreshController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public FreshController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            FreshContent Fresh = await _context.freshContents.FirstOrDefaultAsync();
            return View(Fresh);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FreshContentCreateVM create)
        {
            if (!ModelState.IsValid) return View();

            if (!create.Image.CheckFileType("image/"))
            {
                ModelState.AddModelError("Image", "File must be Image Format");
                return View();
            }
            if (!create.Image.CheckFileSize(200))
            {

                ModelState.AddModelError("Image", "Max File Capacity mut be 200KB");
                return View();
            }
            string fileName = Guid.NewGuid().ToString() + "-" + create.Image.FileName;
            string path = Path.Combine(_env.WebRootPath, "img", fileName);
            await create.Image.SaveFileToLocalAsync(path);
            await _context.freshContents.AddAsync(new FreshContent
            {
                Image = fileName,
                Title = create.Title,
                SubTitle = create.SubTitle,
                PriceFirst = create.PriceFirst,
                PriceSecond = create.PriceSecond,
                Description = create.Description
            });
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();
            FreshContent fresh = await _context.freshContents.Where(c => c.Id == id).FirstOrDefaultAsync();
            if (fresh == null) return NotFound();
            FreshContentDetailVM model = new()
            {
                Id = fresh.Id,
                Title = fresh.Title,
                Image = fresh.Image,
                SubTitle = fresh.SubTitle,
                PriceSecond = fresh.PriceSecond,
                PriceFirst = fresh.PriceFirst,
                Description = fresh.Description,

            };
            return View(model);
        }
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();

            FreshContent fresh = await _context.freshContents.Where(c => c.Id == id).FirstOrDefaultAsync();

            if (fresh is null) return NotFound();

            fresh.Image.DeleteFile(_env.WebRootPath, "img");

            _context.freshContents.Remove(fresh);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null) return BadRequest();
            FreshContent fresh = await _context.freshContents.Where(c => c.Id == id).FirstOrDefaultAsync();
            if (fresh == null) return NotFound();

            return View(new FreshContentEditVM
            {
                Image = fresh.Image,
                Id = fresh.Id,
                Title = fresh.Title,
                SubTitle = fresh.SubTitle,
                PriceSecond = fresh.PriceSecond,
                PriceFirst = fresh.PriceFirst,
                Description = fresh.Description,
            });


        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, FreshContentEditVM request)
        {
            FreshContent fresh = await _context.freshContents.Where(c => c.Id == id).FirstOrDefaultAsync();
            if (!ModelState.IsValid)
            {
                request.Image = fresh.Image;
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
                FileExtensions.DeleteFileFromLocalAsync(Path.Combine(_env.WebRootPath, "img"), fresh.Image);

                string fileName = Guid.NewGuid().ToString() + "-" + request.Photo.FileName;
                string path = Path.Combine(_env.WebRootPath, "img", fileName);
                await request.Photo.SaveFileToLocalAsync(path);

                fresh.Image = fileName;
            }

            if (fresh == null) { return NotFound(); }

            fresh.Title = request.Title;
            fresh.Description = request.Description;
            fresh.SubTitle = request.SubTitle;
            fresh.PriceFirst = request.PriceFirst;
            fresh.PriceSecond = request.PriceSecond;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
    }
}

