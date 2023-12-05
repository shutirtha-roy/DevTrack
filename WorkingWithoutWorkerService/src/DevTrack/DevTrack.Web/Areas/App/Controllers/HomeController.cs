using Microsoft.AspNetCore.Mvc;

namespace DevTrack.Web.Areas.App.Controllers
{
    [Area("App")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Blank()
        {
            return View();
        }
    }
}