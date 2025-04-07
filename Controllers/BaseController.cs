using Microsoft.AspNetCore.Mvc;

namespace Web4.Controllers
{
    public class BaseController : Controller
    {
        public string mainAddress = "https://localhost:7121/";
        public IActionResult Index()
        {
            return View();
        }
    }
}
