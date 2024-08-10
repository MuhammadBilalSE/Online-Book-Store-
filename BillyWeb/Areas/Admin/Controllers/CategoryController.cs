using Billy.DataAccess.Data;
using Billy.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Billy.DataAccess.Repository.IRepository;
using Billy.Utility;
using Microsoft.AspNetCore.Authorization;

namespace BillyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]

    //[Authorize(Roles = SD.role_admin)]
    public class CategoryController : Controller
    {
        private readonly IUnitofWork unitofWork;

        public CategoryController(IUnitofWork _uow)
        {
            unitofWork = _uow;
        }
        public IActionResult Index()
        {
            var categories = unitofWork.CategoryRepository.GetAll();
            return View(categories);
        }
        public IActionResult Create()
        {

            return View();
        }
        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (category.Name == category.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "Name And Display Order can not be same");
            }
            if (category.Name == null || category.Name == "Test".ToLower())
            {
                ModelState.AddModelError("", "Invalid Value for Name");
            }
            if (ModelState.IsValid)
            {
                unitofWork.CategoryRepository.Add(category);
                unitofWork.save();
                TempData["Success"] = "Opertion Successfull";
                return RedirectToAction("Index");
            }
            else
            {
                return View(category);
            }



        }

        public IActionResult Edit(int Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            var categori = unitofWork.CategoryRepository.Get(x => x.Id == Id);
            if (categori == null)
            {
                return NotFound();
            }
            return View(categori);
        }
        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (category.Name == null || category.Name == "Test".ToLower())
            {
                ModelState.AddModelError("", "Invalid Value for Name");
            }
            if (ModelState.IsValid)
            {
                unitofWork.CategoryRepository.update(category);
                unitofWork.save();
                TempData["Success"] = "Opertion Successfull";
                return RedirectToAction("Index");
            }
            else
            {
                return View(category);
            }



        }

        public IActionResult Delete(int id)
        {
            var category = unitofWork.CategoryRepository.Get(x => x.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult Delete(Category? category)
        {
            var dbCategory = unitofWork.CategoryRepository.Get(x => x.Id == category.Id);
            if (category == null)
            {
                return NotFound();
            }
            unitofWork.CategoryRepository.Remove(dbCategory);
            unitofWork.save();
            TempData["Success"] = "Opertion Successfull";
            return RedirectToAction("Index");
        }
    }
}
