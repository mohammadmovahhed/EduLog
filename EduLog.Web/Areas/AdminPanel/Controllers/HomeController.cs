using Microsoft.AspNetCore.Mvc;

namespace EduLog.Web.Areas.AdminPanel.Controllers
{
    [Area("admin")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}