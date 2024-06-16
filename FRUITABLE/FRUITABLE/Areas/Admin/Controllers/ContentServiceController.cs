using FRUITABLE.Data;
using FRUITABLE.Helpers.Extensions;
using FRUITABLE.Models;
using FRUITABLE.ViewModels.ContentService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FRUITABLE.Areas.Admin.Controllers
{


    [Area("Admin")]
    public class ContentServiceController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public ContentServiceController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<ContentService> serviceContent = await _context.contentServices.ToListAsync();
            return View(serviceContent);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ContentServiceCreate create)
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
            await _context.contentServices.AddAsync(new ContentService
            {
                Image = fileName,
                Name = create.Name,
                Description = create.Description
            });
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();
            ContentService serviceContent = await _context.contentServices.Where(c => c.Id == id).FirstOrDefaultAsync();
            if (serviceContent == null) return NotFound();
            ContentServiceDetail model = new()
            {
                Id = serviceContent.Id,
                Name = serviceContent.Name,
                Image = serviceContent.Image,
                Description = serviceContent.Description,

            };
            return View(model);
        }
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();

            ContentService serviceContent = await _context.contentServices.Where(c => c.Id == id).FirstOrDefaultAsync();

            if (serviceContent is null) return NotFound();

            serviceContent.Image.DeleteFile(_env.WebRootPath, "img");

            _context.contentServices.Remove(serviceContent);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null) return BadRequest();
            ContentService serviceContent = await _context.contentServices.Where(c => c.Id == id).FirstOrDefaultAsync();
            if (serviceContent == null) return NotFound();

            return View(new ContentServiceEdit
            {
                Image = serviceContent.Image,
                Name = serviceContent.Name,
                Description = serviceContent.Description
            });


        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, ContentServiceEdit request)
        {
            ContentService serviceContent = await _context.contentServices.Where(c => c.Id == id).FirstOrDefaultAsync();
            if (!ModelState.IsValid)
            {
                request.Image = serviceContent.Image;
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
                FileExtensions.DeleteFileFromLocalAsync(Path.Combine(_env.WebRootPath, "img"), serviceContent.Image);

                string fileName = Guid.NewGuid().ToString() + "-" + request.Photo.FileName;
                string path = Path.Combine(_env.WebRootPath, "img", fileName);
                await request.Photo.SaveFileToLocalAsync(path);

                serviceContent.Image = fileName;
            }

            if (serviceContent == null) { return NotFound(); }

            serviceContent.Name = request.Name;
            serviceContent.Description = request.Description;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
    }
}
