using Microsoft.AspNetCore.Mvc;

namespace FRUITABLE.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult NotFound()
        {
            return View();
        }
    }
}
