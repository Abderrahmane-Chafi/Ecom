using Ecom.DataAccess.Repository.IRepository;
using Ecom.Models;
using Ecom.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace EcomWebApplication.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class BrandController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        public BrandController(IUnitOfWork UnitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _UnitOfWork = UnitOfWork;
            _hostEnvironment = hostEnvironment;

        }

        public IActionResult Index()
        {
            IEnumerable<Brand> objCategoryList = _UnitOfWork.Brand.GetAll();
            return View(objCategoryList);
        }
        //Get
        public IActionResult Create()
        {
            Brand brand = new();
            return View(brand);
        }
        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Brand obj, IFormFile? file)
        {

            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                if (file != null)
                {
                    //Generate new file name
                    string FileName = Guid.NewGuid().ToString();
                    //find the location where the files should be uploaded
                    var uploads = Path.Combine(wwwRootPath, "images","brands");
                    //keep same extension
                    var extension = Path.GetExtension(file.FileName);

                    //update the image - Check if there is an image
                    if (obj.ImageUrl != null)
                    {
                        //old image path
                        var oldImagePath = Path.Combine(wwwRootPath, obj.ImageUrl.TrimStart('/'));
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
                    obj.ImageUrl = "/images/brands/" + FileName + extension;
                }
                _UnitOfWork.Brand.Add(obj);
                _UnitOfWork.Save();
                TempData["success"] = "Brand created successfuly";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        //Get
        public IActionResult Edit(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            var BrandFromDbFirst = _UnitOfWork.Brand.GetFirstOrDefault(u => u.Id == Id);
            if (BrandFromDbFirst == null)
            {
                return NotFound();
            }
            return View(BrandFromDbFirst);
        }
        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Brand obj, IFormFile? file)
        {

            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                if (file != null)
                {
                    //Generate new file name
                    string FileName = Guid.NewGuid().ToString();
                    //find the location where the files should be uploaded
                    var uploads = Path.Combine(wwwRootPath, "images","brands");
                    //keep same extension
                    var extension = Path.GetExtension(file.FileName);

                    //update the image - Check if there is an image
                    if (obj.ImageUrl != null)
                    {
                        //old image path
                        var oldImagePath = Path.Combine(wwwRootPath, obj.ImageUrl.TrimStart('/'));
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
                    obj.ImageUrl = "/images/brands/" + FileName + extension;
                }
                _UnitOfWork.Brand.Update(obj);
                _UnitOfWork.Save();
                TempData["success"] = "Brand edited successfuly";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        //Get
        public IActionResult Delete(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            //var CategoryFromDb = _db.Categories.Find(Id);
            var BrandFromDbFirst = _UnitOfWork.Brand.GetFirstOrDefault(u => u.Id == Id);
            if (BrandFromDbFirst == null)
            {
                return NotFound();
            }
            return View(BrandFromDbFirst);
        }
        //Post
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? Id)
        {
            var Obj = _UnitOfWork.Brand.GetFirstOrDefault(u => u.Id == Id);
            if (Obj == null)
            {
                return NotFound();
            }
            var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, Obj.ImageUrl.TrimStart('/'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }
            _UnitOfWork.Brand.Remove(Obj);
            _UnitOfWork.Save();
            TempData["success"] = "Brand deleted successfuly";
            return RedirectToAction("Index");
        }
    }
}
