using Ecom.DataAccess.Repository;
using Ecom.DataAccess.Repository.IRepository;
using Ecom.Models;
using Ecom.Models.ViewModels;
using Ecom.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Protocol.Plugins;
using System.Data;

namespace EcomWebApplication.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]

    public class ProductController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;


        public ProductController(IUnitOfWork UnitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _UnitOfWork = UnitOfWork;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        //Get
        public IActionResult Upsert(int? Id)
        {
            var genders = new List<string>() { "Male", "Female" };
            var status = new List<string>() { "Available", "Out of stock" };

            ProductVM productVM = new()
            {
                product = new(),
                CategoryList = _UnitOfWork.Category.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                GenderList=genders.Select(i => new SelectListItem {
                    Text = i,
                    Value = i
                })
                ,
                BrandList = _UnitOfWork.Brand.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                sizes=_UnitOfWork.Sizes.GetAll().ToList(),
                productDetails= new List<ProductDetails>(),
                Othersizes = new List<Sizes>(),
                StatusList= status.Select(i => new SelectListItem
                {
                    Text = i,
                    Value = i
                })

            };
            if (Id == null || Id == 0)
            {
                //Create product
                return View(productVM);
            }
            else
            {
                //Update product
                productVM.product = _UnitOfWork.Product.GetFirstOrDefault(u => u.Id == Id,includeProperties:"Category");
                productVM.productDetails = _UnitOfWork.ProductDetails.GetAll(u => u.ProductId == productVM.product.Id,includeProperties:"Sizes").ToList();
                if (productVM.product.Category.Name == "Shoes")
                    productVM.sizes = _UnitOfWork.Sizes.GetAll(s => s.type == "Shoes");
                else
                    productVM.sizes = _UnitOfWork.Sizes.GetAll().Where(s => s.type == "Clothing").ToList();
                List<Sizes> NotIncluded = new List<Sizes>();
                foreach (var item in productVM.sizes)
                {
                    bool isIncluded = false;
                    foreach (var l1 in productVM.productDetails)
                    {
                        if (l1.Sizes.Id == item.Id)
                        {
                            isIncluded = true;
                            break;
                        }
                    }

                    if (!isIncluded)
                    {
                        NotIncluded.Add(item);
                    }
                }
                productVM.Othersizes = NotIncluded;
                return View(productVM);
            }

        }
        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM obj, IFormFile? file, IFormFile? file1, IFormCollection form)
        {

            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                if (file != null)
                {
                    //Generate new file name
                    string FileName = Guid.NewGuid().ToString();
                    //find the location where the files should be uploaded
                    var uploads = Path.Combine(wwwRootPath, "images","products");
                    //keep same extension
                    var extension = Path.GetExtension(file.FileName);

                    //update the image - Check if there is an image
                    if (obj.product.ImageUrl != null)
                    {
                        //old image path
                        var oldImagePath = Path.Combine(wwwRootPath, obj.product.ImageUrl.TrimStart('/'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    //copy the file uploaded inside the product folder
                    using (var fileStreams = new FileStream(Path.Combine(uploads, FileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    //what we will save in the DB
                    obj.product.ImageUrl = "/images/products/" + FileName + extension;
                }
                if (file1 != null)
                {
                    //Generate new file name
                    string FileName = Guid.NewGuid().ToString();
                    //find the location where the files should be uploaded
                    var uploads = Path.Combine(wwwRootPath, "images","products");
                    //keep same extension
                    var extension = Path.GetExtension(file1.FileName);

                    //update the image - Check if there is an image
                    if (obj.product.ImageUrl1 != null)
                    {
                        //old image path
                        var oldImagePath = Path.Combine(wwwRootPath, obj.product.ImageUrl1.TrimStart('/'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    //copy the file uploaded inside the product folder
                    using (var fileStreams = new FileStream(Path.Combine(uploads, FileName + extension), FileMode.Create))
                    {
                        file1.CopyTo(fileStreams);
                    }
                    //what we will save in the DB
                    obj.product.ImageUrl1 = "/images/products/" + FileName + extension;
                }

                if (obj.product.Id == 0)
                {
                    _UnitOfWork.Product.Add(obj.product);
                    _UnitOfWork.Save();
                    //Create Renting_Services
                    List<int> checkedIds = new List<int>();
                    obj.sizes = _UnitOfWork.Sizes.GetAll();
                    foreach (var item in obj.sizes)
                    {
                        string checkboxName = $"MyCheckboxes_{item.Id}";
                        if (form.TryGetValue(checkboxName, out var value) && value == "true")
                        {
                            checkedIds.Add(item.Id);

                            ProductDetails productDetails = new ProductDetails
                            {
                                ProductId = obj.product.Id,
                                SizeId = item.Id
                            };

                            _UnitOfWork.ProductDetails.Add(productDetails);
                            _UnitOfWork.Save();
                        }
                    }
                    TempData["success"] = "product created successfuly";

                }
                else
                {
                    var currentCategory = _UnitOfWork.Category.GetFirstOrDefault(u => u.Id == obj.product.CategoryId);
                    _UnitOfWork.Product.Update(obj.product);
                    if(currentCategory.Name== "Shoes")
                        obj.sizes = _UnitOfWork.Sizes.GetAll(s => s.type == "Shoes");
                    else
                        obj.sizes = _UnitOfWork.Sizes.GetAll().Where(s => s.type == "Clothing").ToList();
                    List<int> checkedIds = new List<int>();
                    foreach (var item in obj.sizes)
                    {
                        string checkboxName = $"MyCheckboxes_{item.Id}";
                        if (form.TryGetValue(checkboxName, out var value) && value == "true")
                        {
                            checkedIds.Add(item.Id);

                            ProductDetails productDetails = new ProductDetails
                            {
                                ProductId = obj.product.Id,
                                SizeId = item.Id
                            };

                            _UnitOfWork.ProductDetails.Add(productDetails);
                            _UnitOfWork.Save();
                        }
                    }
                }
                _UnitOfWork.Save();
                TempData["success"] = "product edited successfuly";
                return RedirectToAction("Index");
            }
            return View(obj);
        }
      


        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var productList = _UnitOfWork.Product.GetAll(includeProperties: "Category,Brand");
            return Json(new { data = productList });
        }
        //POST
        [HttpDelete]
        public IActionResult Delete(int? Id)
        {
            var Obj = _UnitOfWork.Product.GetFirstOrDefault(u => u.Id == Id);
            if (Obj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, Obj.ImageUrl.TrimStart('/'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }
            _UnitOfWork.Product.Remove(Obj);
            _UnitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });

        }
        [HttpDelete]
        public IActionResult DeleteProductDetail(int? Id)
        {
            var Obj = _UnitOfWork.ProductDetails.GetFirstOrDefault(u => u.Id == Id);
            if (Obj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _UnitOfWork.ProductDetails.Remove(Obj);
            _UnitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });

        }
        [HttpGet]
        public IActionResult GetSizesByCategory(int categoryId)
        {
            Category CategoryFromDb = _UnitOfWork.Category.GetFirstOrDefault(u => u.Id == categoryId);
            var sizes = _UnitOfWork.Sizes.GetAll();
            if (CategoryFromDb.Name=="Shoes")
                 sizes = _UnitOfWork.Sizes.GetAll().Where(s=>s.type == "Shoes").ToList();
            else
                sizes = _UnitOfWork.Sizes.GetAll().Where(s => s.type == "Clothing").ToList();

            return Json(sizes);
        }

        #endregion
    }
}
