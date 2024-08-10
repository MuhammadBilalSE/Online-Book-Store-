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
    public class ProductController : Controller
    {
        private readonly IUnitofWork work;
        private readonly IWebHostEnvironment webHost ;
        public ProductController(IUnitofWork _work, IWebHostEnvironment _webHost)
        {
            work = _work;
            webHost = _webHost;
        }
        public IActionResult List()
        {
            List<Product> products = work.ProductRepository.GetAll(includeProperties:"category").ToList();
            
            return View(products);
        }
        public IActionResult Upsert(int? id)
        {
            ProductVM productvm = new ProductVM();

			IEnumerable<SelectListItem> listcat = work.CategoryRepository.GetAll().
				Select(u => new SelectListItem
				{
					Text = u.Name,
					Value = u.Id.ToString()
				});
            productvm.CategoryList= listcat;
            productvm.product = new Product();

            if (id == 0 || id == null)
            {
                return View(productvm);
            }
            else{
                productvm.product = work.ProductRepository.Get(x => x.Id == id);
                return View(productvm);
            }
        }
        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)
             {
            var wwwpath = webHost.WebRootPath;
            if (file!=null)
            {
                var filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName) ;
                var productpath = Path.Combine(wwwpath, @"Images\Product");
                if (!string.IsNullOrEmpty(productVM.product.ImgUrl))
                {
                    var oldpath = Path.Combine(wwwpath, productVM.product.ImgUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(oldpath))
                    {
                        System.IO.File.Delete(oldpath);
                    }
                }
                using (var filestream = new FileStream(Path.Combine(productpath, filename), FileMode.Create))
                {
                    file.CopyTo(filestream);
                }
                productVM.product.ImgUrl = @"Images\Product\" + filename;
            }
            
            if (ModelState.IsValid)
            {
                if (productVM.product.Id == 0)
                {

                    work.ProductRepository.Add(productVM.product);
                }
                else 
                { 
                work.ProductRepository.update(productVM.product);
                }
                work.save();
                TempData["Success"] = "Opertion Successfull";
                return RedirectToAction("List");
            }
            else
            {

                IEnumerable<SelectListItem> listcat = work.CategoryRepository.GetAll().
                    Select(u => new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.Id.ToString()
                    });
                productVM.CategoryList = listcat;
                return View(productVM);
            }



        }



        //public IActionResult Delete(int id)
        //{
        //    var product = work.ProductRepository.Get(x => x.Id == id);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(product);
        //}

        #region APICALLS
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var dbproduct = work.ProductRepository.Get(x => x.Id == id);
            if (dbproduct == null)
            {
                return Json(new { success = false, ErrorMessage= "Not Found" }) ;
            }
			if (!string.IsNullOrEmpty(dbproduct.ImgUrl))
			{
				var oldpath = Path.Combine(webHost.WebRootPath,
                    dbproduct.ImgUrl.TrimStart('\\'));
				if (System.IO.File.Exists(oldpath))
				{
					System.IO.File.Delete(oldpath);
				}
			}
			work.ProductRepository.Remove(dbproduct);
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
            List<Product> products = work.ProductRepository.GetAll(includeProperties: "category").ToList();

            return Json(new { data = products });
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
//    var product = work.ProductRepository.Get(x => x.Id == Id);
//    if (product == null)
//    {
//        return NotFound();
//    }
//    return View(product);
//}
//[HttpPost]
//public IActionResult Edit(Product product)
//{
//    if (ModelState.IsValid)
//    {
//        work.ProductRepository.update(product);
//        work.save();
//        TempData["Success"] = "Opertion Successfull";
//        return RedirectToAction("Index");
//    }
//    else
//    {
//        return View(product);
//    }



//}