using Microsoft.AspNetCore.Mvc;

namespace RRS.Areas.Member.Controllers
{
    public class HomeController : Controller
    {
        [Area("Member")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
