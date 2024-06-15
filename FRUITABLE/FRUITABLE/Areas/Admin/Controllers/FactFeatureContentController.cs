using FRUITABLE.Data;
using FRUITABLE.Models;
using FRUITABLE.ViewModels.FactFeatureContent;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FRUITABLE.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FactFeatureContentController : Controller
    {
        private readonly AppDbContext _context;
        public FactFeatureContentController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<FactFeatureContent> factFeatureContents = await _context.factFeatureContents.ToListAsync();
            return View(factFeatureContents);

        }


        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();
            FactFeatureContent factContent = await _context.factFeatureContents.Where(c => c.Id == id).FirstOrDefaultAsync();
            if (factContent == null) return NotFound();
            FactFeatureContentDetailVM model = new()
            {
                Title = factContent.Title,
                NumberInfo = factContent.NumberInfo,
                Icon = factContent.Icon

            };
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FactFeatureContent fact)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            bool existfact = await _context.factFeatureContents.AnyAsync(m => m.Title == fact.Title
                                                           && m.Icon == fact.Icon
                                                           && m.NumberInfo == fact.NumberInfo);
            if (existfact)
            {
                ModelState.AddModelError("Title", "These inputs already exist");
            }

            await _context.factFeatureContents.AddAsync(new FactFeatureContent
            {
                Title = fact.Title,
                NumberInfo = fact.NumberInfo,
                Icon = fact.Icon

            });
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }



        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            FactFeatureContent factContent = await _context.factFeatureContents.Where(c => c.Id == id).FirstOrDefaultAsync();

            if (factContent == null) return NotFound();

            _context.factFeatureContents.Remove(factContent);
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
            FactFeatureContent factContent = await _context.factFeatureContents.Where(c => c.Id == id).FirstOrDefaultAsync();

            if (factContent == null) return NotFound();

            return View(new FactFeatureContentEditVM
            {
                Title = factContent.Title,
                Icon = factContent.Icon,
                NumberInfo = factContent.NumberInfo
            });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(FactFeatureContentEditVM update, int? id)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (id == null) return BadRequest();
            FactFeatureContent factContent = await _context.factFeatureContents.Where(c => c.Id == id).FirstOrDefaultAsync();

            if (update == null) return NotFound();

            factContent.Title = update.Title;
            factContent.Icon = update.Icon;
            factContent.NumberInfo = update.NumberInfo;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }
    }
}
