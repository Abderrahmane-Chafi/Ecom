using Ecom.DataAccess.Repository.IRepository;
using Ecom.Models;
using Ecom.Models.ViewModels;
using Ecom.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using NuGet.Protocol.Plugins;
using System.Security.Claims;

namespace EcomWebApplication.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize]
	public class OrderController : Controller
	{
		private readonly IUnitOfWork _UnitOfWork;
		[BindProperty]
		public OrderVM OrderVM { get; set; }
		public OrderController(IUnitOfWork UnitOfWork)
		{
			_UnitOfWork = UnitOfWork;
		}

		public IActionResult Index()
		{
			return View();
		}
        //Get
        public IActionResult Details(int orderId)
        {
            var status = new List<string>() { "Processing", "Shipped" };
            OrderVM = new OrderVM()
            {
                OrderHeader = _UnitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == orderId, includeProperties: "ApplicationUser"),
                OrderDetail = _UnitOfWork.OrderDetail.GetAll(u => u.OrderId == orderId, includeProperties: "Product"),
                orderStatus= status.Select(i => new SelectListItem
				{
					Text = i,
					Value = i
				})
			};
            return View(OrderVM);
        }
		[HttpPost]
		[Authorize(Roles = SD.Role_Admin)]
		[ValidateAntiForgeryToken]
		public IActionResult UpdateOrderDetail()
		{
			var orderHeaderFromDb = _UnitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == OrderVM.OrderHeader.Id, tracked: false);
			//We don't want so update all the properties
			orderHeaderFromDb.Name = OrderVM.OrderHeader.Name;
			orderHeaderFromDb.PhoneNumber = OrderVM.OrderHeader.PhoneNumber;
			orderHeaderFromDb.StreetAdress = OrderVM.OrderHeader.StreetAdress;
			orderHeaderFromDb.City = OrderVM.OrderHeader.City;
			orderHeaderFromDb.OrderStatus = OrderVM.OrderHeader.OrderStatus;
			orderHeaderFromDb.PostalCode = OrderVM.OrderHeader.PostalCode;
			orderHeaderFromDb.ShippingDate = DateTime.SpecifyKind(OrderVM.OrderHeader.ShippingDate, DateTimeKind.Utc);

            //Entity Framwork is tracking the entity when we retrieve it
            //Entity Framwork have an option when you retrieve an entity you can explicitly say that you do not want to track that entiy
            //If the entity is not tracked then update will not work even if we update properties(without .update)
            _UnitOfWork.OrderHeader.Update(orderHeaderFromDb);
			_UnitOfWork.Save();
			TempData["success"] = "Order Details updated successfuly";
			return RedirectToAction("Details", "Order", new { orderId = orderHeaderFromDb.Id });
		}
		[HttpPost]
		[Authorize(Roles = SD.Role_Admin)]
		[ValidateAntiForgeryToken]
		public IActionResult CancelOrder()
		{
			var orderHeader = _UnitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == OrderVM.OrderHeader.Id, tracked: false);

			//Payment isn't done
			_UnitOfWork.OrderHeader.UpdateStatus(orderHeader.Id, SD.StatusCancelled, SD.StatusCancelled); _UnitOfWork.Save();
			TempData["success"] = "Order cancelled successfuly";
			return RedirectToAction("Details", "Order", new { orderId = OrderVM.OrderHeader.Id });
		}
		#region API CALLS
		[HttpGet]
        public IActionResult GetAll()
        {
            IEnumerable<OrderHeader> orderHeaders;
            if (User.IsInRole(SD.Role_Admin))
            {
                orderHeaders = _UnitOfWork.OrderHeader.GetAll(includeProperties: "ApplicationUser");
            }
            else
            {
                var claimsIDentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIDentity.FindFirst(ClaimTypes.NameIdentifier);
                orderHeaders = _UnitOfWork.OrderHeader.GetAll(u => u.ApplicationUserId == claim.Value, includeProperties: "ApplicationUser");
            }
            return Json(new { data = orderHeaders });
        }
        #endregion
    }
}
