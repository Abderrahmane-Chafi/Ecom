using Ecom.DataAccess.Repository.IRepository;
using Ecom.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EcomWebApplication.ViewComponents
{
    public class ShoppingCartViewComponent : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;
        public ShoppingCartViewComponent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        //In unitOfWork we have the same method
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var claimsIDentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIDentity.FindFirst(ClaimTypes.NameIdentifier);
            //If user is logged in
            if (claim != null)
            {
                //Check if session is null or not
                if (HttpContext.Session.GetInt32(SD.SessionCart) != null)
                {
                    return View(HttpContext.Session.GetInt32(SD.SessionCart));
                }
                else
                {
                    //REtrieve the count
                    HttpContext.Session.SetInt32(SD.SessionCart,
                        _unitOfWork.ShoppingCart.GetAll(U => U.ApplicationUserId == claim.Value).ToList().Count);
                    return View(HttpContext.Session.GetInt32(SD.SessionCart));
                }
            }
            //User not logged
            else
            {
                HttpContext.Session.Clear();
                return View(0);
            }
        }
    }
}
