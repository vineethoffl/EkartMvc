using Microsoft.AspNetCore.Mvc;

namespace EkartMvc.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult OrderDetails()
        {
            return View();
        }
    }
}
