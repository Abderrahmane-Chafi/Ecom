using Ecom.DataAccess.Repository;
using Ecom.DataAccess.Repository.IRepository;
using Ecom.Models.ViewModels;
using EcomWebApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace EcomWebApplication.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _UnitOfWork;

        public HomeController(IUnitOfWork UnitOfWork, ILogger<HomeController> logger)
        {
            _logger = logger;
            _UnitOfWork = UnitOfWork;

        }

        public IActionResult Index()
        {
            HomeVM homeVM = new()
            {
                Brands = _UnitOfWork.Brand.GetAll().ToList(),
                Product1 = _UnitOfWork.Product.GetFirstOrDefault(u=>u.Brand.Name=="Fred Perry",includeProperties:"Brand"),
                Product2 = _UnitOfWork.Product.GetFirstOrDefault(u => u.Brand.Name == "Lyle and Scott", includeProperties: "Brand"),
                Product3 = _UnitOfWork.Product.GetFirstOrDefault(u => u.Brand.Name == "Ellesse", includeProperties: "Brand"),
                Feedback = new(),
                Feedbacks=_UnitOfWork.FeedBack.GetAll(includeProperties:"ApplicationUser").Take(3).ToList()
            };
            return View(homeVM);
        }
        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Index(HomeVM homeVM)
        {
            var claimsIDentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIDentity.FindFirst(ClaimTypes.NameIdentifier);
            if (homeVM.Feedback.Comment != null)
            {
                homeVM.Feedback.ApplicationUserId = claim.Value;
                homeVM.Feedback.Date = DateTime.SpecifyKind(homeVM.Feedback.Date, DateTimeKind.Utc);
                _UnitOfWork.FeedBack.Add(homeVM.Feedback);
                TempData["success"] = "Thanks for sharing your feedback";
                _UnitOfWork.Save();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["error"] = "Error: Feedback is empty";
                return RedirectToAction("Index", "Home");
            }
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}