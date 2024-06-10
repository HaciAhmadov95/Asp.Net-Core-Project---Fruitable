
using Microsoft.AspNetCore.Mvc;

namespace FRUITABLE.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}