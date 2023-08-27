using Microsoft.AspNetCore.Mvc;

namespace EcomWebApplication.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class AboutUsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
