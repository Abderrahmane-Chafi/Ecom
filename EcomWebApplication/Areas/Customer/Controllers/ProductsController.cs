using Ecom.DataAccess.Repository;
using Ecom.DataAccess.Repository.IRepository;
using Ecom.Models;
using Ecom.Models.ViewModels;
using Ecom.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using X.PagedList;

namespace EcomWebApplication.Areas.Customer.Controllers
{
    [Area("Customer")]

    public class ProductsController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;
        public ProductsController(IUnitOfWork UnitOfWork)
        {
            _UnitOfWork = UnitOfWork;

        }
        public IActionResult Index(int? page, List<string> BrandFilter, List<string> CategoryFilter, double price)
        {
            int pageSize = 12; // Number of products to display per page
            int pageNumber = page ?? 1; // Current page number
            var ProductsList = _UnitOfWork.Product.GetAll(u=>u.status=="Available", includeProperties: "Brand,Category");

            // Apply filters
            var filteredProducts = ProductsList;
            if (BrandFilter != null && BrandFilter.Any())
            {
                filteredProducts = filteredProducts.Where(c => BrandFilter.Contains(c.Brand.Name));
            }
            if (CategoryFilter != null && CategoryFilter.Any())
            {
                filteredProducts = filteredProducts.Where(c => CategoryFilter.Contains(c.Category.Name));
            }
            if (price > 0)
            {
                filteredProducts = filteredProducts.Where(c => c.Price <= price);
            }

            var pagedProducts = filteredProducts.ToPagedList(pageNumber, pageSize);

            ProductsVM productsVM = new()
            {
                products = filteredProducts,
                brands = _UnitOfWork.Brand.GetAll(),
                Price = price,
                PagedProducts = pagedProducts,
                SelectedBrandFilters = BrandFilter,
                categories=_UnitOfWork.Category.GetAll(),
                SelectedCategoriesFilters=CategoryFilter
            };

            return View(productsVM);
        }

        public IActionResult Details(int ProductId)
        {
            DetailsVM detailsVM = new()
            {
                cartObj = new()
                {
                    Count = 1,
                    ProductId = ProductId,
                    Product = _UnitOfWork.Product.GetFirstOrDefault(u => u.Id == ProductId, includeProperties: "Category,Brand")
                },
                productDetails=new List<ProductDetails>()
                
            };
            detailsVM.productDetails = _UnitOfWork.ProductDetails.GetAll(u => u.ProductId == ProductId,includeProperties:"Sizes");
            return View(detailsVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        //Enforce the user to log in to access the post access method
        [Authorize]
        public IActionResult Details(DetailsVM detailsVM)
        {
            //the user object is always available by default
            var claimsIDentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIDentity.FindFirst(ClaimTypes.NameIdentifier);
            detailsVM.cartObj.ApplicationUserId = claim.Value;
            //Retrieve the existing entry and modify the count 
            ShoppingCart cartFromDb = _UnitOfWork.ShoppingCart.GetFirstOrDefault(
                u => u.ApplicationUserId == claim.Value && u.ProductId == detailsVM.cartObj.ProductId, includeProperties: "Product"
                );
            if (cartFromDb == null)
            {
                _UnitOfWork.ShoppingCart.Add(detailsVM.cartObj);
                _UnitOfWork.Save();
                TempData["success"] = "Product added succesfuly to cart!";

                HttpContext.Session.SetInt32(SD.SessionCart,
                    _UnitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value).ToList().Count);

            }
            else
            {
                //Method to update count
                if (cartFromDb.Count < cartFromDb.Product.Quantity)
                {
                    _UnitOfWork.ShoppingCart.IncrementCount(cartFromDb, detailsVM.cartObj.Count);
                }
                else
                {
                    TempData["error"] = "The count is over than the quantity";

                }
                _UnitOfWork.Save();
            }


            return RedirectToAction(nameof(Index));
        }
    }
}
