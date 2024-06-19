using FRUITABLE.Data;
using Microsoft.AspNetCore.Mvc;

namespace FRUITABLE.Controllers
{
    public class ContactController : Controller
    {
        private readonly AppDbContext _context;
        public ContactController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
