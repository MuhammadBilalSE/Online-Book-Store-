using Billy.DataAccess.Repository.IRepository;
using Billy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace BillyWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitofWork work;

        public HomeController(IUnitofWork _work, ILogger<HomeController> logger)
        {
            work = _work;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var list = work.ProductRepository.GetAll(includeProperties:"category").ToList();
            return View(list);
        }
        public IActionResult Details(int productId)
        {
            var cart = new Cart();
            cart.product = work.ProductRepository.Get(x =>x.Id == productId, includeProperties: "category");
            cart.Count = 1;
            cart.ProductId = productId;
            return View(cart);
        }
        [HttpPost]
        [Authorize]
        public IActionResult Details(Cart cart)
        {
            var userClaims = (ClaimsIdentity)User.Identity;
            var user = userClaims.FindFirst(ClaimTypes.NameIdentifier).Value;
            cart.CustomerId = user;
            var dbCart = work.CartRepository.Get(x=> x.Id == cart.Id && x.CustomerId == user);
            if (dbCart == null)
            {
                if (cart.Count != null)
                {
                    work.CartRepository.Add(cart);
                    work.save();
                    return RedirectToAction("Index","Cart");
                }
            }
            else
            {
                dbCart.Count += cart.Count;
                work.CartRepository.update(dbCart);
                work.save();
                return RedirectToAction("Index");
            }
            return View(cart);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
