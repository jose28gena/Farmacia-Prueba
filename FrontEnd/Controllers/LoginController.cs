using Microsoft.AspNetCore.Mvc;

namespace FrontEnd.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
