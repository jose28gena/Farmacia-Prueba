using Microsoft.AspNetCore.Mvc;

namespace FrontEnd.Controllers
{
    public class MedicamentosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
