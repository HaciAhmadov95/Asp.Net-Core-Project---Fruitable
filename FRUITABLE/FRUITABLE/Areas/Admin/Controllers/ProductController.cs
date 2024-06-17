using Microsoft.AspNetCore.Mvc;

namespace FRUITABLE.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
