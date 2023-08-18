using Ecom.DataAccess.Repository.IRepository;
using Ecom.Models;
using Ecom.Models.ViewModels;
using Ecom.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace EcomWebApplication.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class SizeController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;

        public SizeController(IUnitOfWork UnitOfWork)
        {
            _UnitOfWork = UnitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Sizes> obj = _UnitOfWork.Sizes.GetAll();
            return View(obj);
        }
        //Get
        public IActionResult Create()
        {
            var types = new List<string>()
            {
                "Clothing",
                "Shoes"
            };
            SizeVM sizeVM = new()
            {
                size = new(),
                SizeTypes = types.Select(i => new SelectListItem
                {
                    Text = i,
                    Value = i
                })
            };
            return View(sizeVM);
        }
        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(SizeVM obj)
        {

            if (ModelState.IsValid)
            {
                _UnitOfWork.Sizes.Add(obj.size);
                _UnitOfWork.Save();
                TempData["success"] = "Size created successfuly";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        //Get
        public IActionResult Edit(int? Id)
        {
            var types = new List<string>()
            {
                "Clothing",
                "Shoes"
            };
            SizeVM sizeVM = new()
            {
                size = _UnitOfWork.Sizes.GetFirstOrDefault(u => u.Id == Id),
                SizeTypes = types.Select(i => new SelectListItem
                {
                    Text = i,
                    Value = i
                })
            };
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            if (sizeVM.size == null)
            {
                return NotFound();
            }
            return View(sizeVM);
        }
        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(SizeVM obj)
        {

            if (ModelState.IsValid)
            {
                _UnitOfWork.Sizes.Update(obj.size);
                _UnitOfWork.Save();
                TempData["success"] = "Size edited successfuly";
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
            var SizeFromDbFirst = _UnitOfWork.Sizes.GetFirstOrDefault(u => u.Id == Id);
            if (SizeFromDbFirst == null)
            {
                return NotFound();
            }
            return View(SizeFromDbFirst);
        }
        //Post
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? Id)
        {
            var Obj = _UnitOfWork.Sizes.GetFirstOrDefault(u => u.Id == Id);
            if (Obj == null)
            {
                return NotFound();
            }
            _UnitOfWork.Sizes.Remove(Obj);
            _UnitOfWork.Save();
            TempData["success"] = "Size deleted successfuly";
            return RedirectToAction("Index");
        }
    }
}
