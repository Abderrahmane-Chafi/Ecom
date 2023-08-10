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
            return View();
        }
        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Sizes obj)
        {

            if (ModelState.IsValid)
            {
                _UnitOfWork.Sizes.Add(obj);
                _UnitOfWork.Save();
                TempData["success"] = "Size created successfuly";
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
            //var CategoryFromDb = _db.Categories.Find(Id);
            var SizeFromDbFirst = _UnitOfWork.Sizes.GetFirstOrDefault(u => u.Id == Id);
            if (SizeFromDbFirst == null)
            {
                return NotFound();
            }
            return View(SizeFromDbFirst);
        }
        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Sizes obj)
        {

            if (ModelState.IsValid)
            {
                _UnitOfWork.Sizes.Update(obj);
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
