using Ecom.DataAccess.Repository.IRepository;
using Ecom.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcomWebApplication.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class FeedBackController : Controller
    {
        public readonly IUnitOfWork _UnitOfWork;
        public FeedBackController(IUnitOfWork UnitOfWork)
        {
            _UnitOfWork = UnitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var List = _UnitOfWork.FeedBack.GetAll(includeProperties: "ApplicationUser");
            return Json(new { data = List });
        }
        #endregion
    }
}
