using Microsoft.AspNetCore.Mvc;
using PlagiarismSystem.Filters;


namespace PlagiarismSystem.Areas.Main.Controllers
{
    [Area("Main")]
    [ResourceFilter]
    public class MainController : Controller
    {
        [HttpGet]
        public IActionResult Main()
        {
            return View();
        }
    }
}