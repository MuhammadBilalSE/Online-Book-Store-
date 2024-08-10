using Billy.DataAccess.Repository;
using Billy.DataAccess.Repository.IRepository;
using Billy.Models;
using Billy.Models.ViewModels;
using Billy.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;

namespace BillyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles =SD.role_admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitofWork work;
        private readonly IWebHostEnvironment webHost ;
        public CompanyController(IUnitofWork _work, IWebHostEnvironment _webHost)
        {
            work = _work;
            webHost = _webHost;
        }
        public IActionResult List()
        {
            List<Company> Companys = work.CompanyRepository.GetAll().ToList();
            
            return View(Companys);
        }
        public IActionResult Upsert(int? id)
        {
            
            if (id == 0 || id == null)
            {
                return View(new Company());
            }
            else
            {
                Company company = work.CompanyRepository.Get(x => x.Id == id);
                return View(company);
            }
        }
        [HttpPost]
        public IActionResult Upsert(Company company)
             {
            
            
            if (ModelState.IsValid)
            {
                if (company.Id == 0)
                {

                    work.CompanyRepository.Add(company);
                }
                else 
                { 
                work.CompanyRepository.update(company);
                }
                work.save();
                TempData["Success"] = "Opertion Successfull";
                return RedirectToAction("List");
            }
            else
            {
                return View(company);
            }



        }
        #region APICALLS
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var dbCompany = work.CompanyRepository.Get(x => x.Id == id);
            if (dbCompany == null)
            {
                return Json(new { success = false, ErrorMessage= "Not Found" }) ;
            }
			work.CompanyRepository.Remove(dbCompany);
            work.save();
            TempData["Success"] = "Opertion Successfull";
            return Json(new
            {
                Success = true,
                Message = "Deleted Successfuly"
            });
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> Companys = work.CompanyRepository.GetAll().ToList();

            return Json(new { data = Companys });
        }
        #endregion
    }
}
//public IActionResult Edit(int Id)
//{
//    if (Id == null || Id == 0)
//    {
//        return NotFound();
//    }
//    var Company = work.CompanyRepository.Get(x => x.Id == Id);
//    if (Company == null)
//    {
//        return NotFound();
//    }
//    return View(Company);
//}
//[HttpPost]
//public IActionResult Edit(Company Company)
//{
//    if (ModelState.IsValid)
//    {
//        work.CompanyRepository.update(Company);
//        work.save();
//        TempData["Success"] = "Opertion Successfull";
//        return RedirectToAction("Index");
//    }
//    else
//    {
//        return View(Company);
//    }



//}

//public IActionResult Delete(int id)
//{
//    var Company = work.CompanyRepository.Get(x => x.Id == id);
//    if (Company == null)
//    {
//        return NotFound();
//    }
//    return View(Company);
//}
