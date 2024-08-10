using Billy.DataAccess.Repository.IRepository;
using Billy.Models;
using Billy.Models.ViewModels;
using Billy.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.BillingPortal;
using Stripe.Checkout;
using System.Security.Claims;

namespace BillyWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitofWork unitofwork;
        public CartVM cartVM;
        public CartController(IUnitofWork _unit)
        {
            unitofwork = _unit;
        }
        public IActionResult Index()
        {
            var userclaim = (ClaimsIdentity)User.Identity;
            var user = userclaim.FindFirst(ClaimTypes.NameIdentifier).Value;

            cartVM = new CartVM();
            cartVM.OrderHeader = new OrderHeader();
            cartVM.CartList = unitofwork.CartRepository.GetAll(x => x.CustomerId == user, includeProperties: "product");

            foreach (var item in cartVM.CartList)
            {
                item.Price = CalculatePrice(item);
                cartVM.OrderHeader.OrderTotal += (item.Price * item.Count);
            }

            return View(cartVM);
        }

        public IActionResult Summary()
        {
            var userclaim = (ClaimsIdentity)User.Identity;
            var user = userclaim.FindFirst(ClaimTypes.NameIdentifier).Value;

            cartVM = new CartVM();

            cartVM.CartList = unitofwork.CartRepository.GetAll(x => x.CustomerId == user, includeProperties: "product");
            cartVM.OrderHeader = new OrderHeader();
            cartVM.OrderHeader.AppUser = unitofwork.AppUserRepository.Get(x => x.Id == user);
            cartVM.OrderHeader.Name = cartVM.OrderHeader.AppUser.Name;
            cartVM.OrderHeader.Address = cartVM.OrderHeader.AppUser.Address;
            cartVM.OrderHeader.City = cartVM.OrderHeader.AppUser.City;
            cartVM.OrderHeader.State = cartVM.OrderHeader.AppUser.State;
            cartVM.OrderHeader.PhoneNumber = cartVM.OrderHeader.AppUser.PhoneNumber;
            cartVM.OrderHeader.PostalCode = cartVM.OrderHeader.AppUser.PostalCode;
            foreach (var item in cartVM.CartList)
            {
                item.Price = CalculatePrice(item);
                cartVM.OrderHeader.OrderTotal += (item.Price * item.Count);
            }

            return View(cartVM);
        }

        [HttpPost]
		public ActionResult Summary(CartVM cart)
		{
			var userclaim = (ClaimsIdentity)User.Identity;
			var user = userclaim.FindFirst(ClaimTypes.NameIdentifier).Value;
            cart.CartList = unitofwork.CartRepository.GetAll (x => x.CustomerId == user, includeProperties: "product");
            cart.OrderHeader.OrderDate = System.DateTime.Now;
            cart.OrderHeader.UserId = user;
            AppUser appUser = unitofwork.AppUserRepository.Get(x => x.Id == user);

			foreach (var item in cart.CartList)
			{
				item.Price = CalculatePrice(item);
				cart.OrderHeader.OrderTotal += (item.Price * item.Count);
			}
			if (appUser.CompanyId.GetValueOrDefault()==0)
            {
                cart.OrderHeader.PaymentStatus = SD.PaymentPending.ToString();
				cart.OrderHeader.OrderStatus = SD.StatusPending.ToString();
            }
            else
            {
				cart.OrderHeader.PaymentStatus = SD.PaymentDelayedDone.ToString();
				cart.OrderHeader.OrderStatus = SD.StatusApproved.ToString();
			}
            unitofwork.OrderHeaderRepository.Add(cart.OrderHeader);
            unitofwork.save();

            foreach(var item in cart.CartList)
            {
                OrderDetail detail = new OrderDetail()
                {
                    ProductId    = item.ProductId,
                    Count        = item.Count,
                    Price        = item.Price,
                    OrderHeaderId= cart.OrderHeader.Id
                };
                unitofwork.OrderDetailsRepository.Add (detail);
                unitofwork.save();
            }

			if (appUser.CompanyId.GetValueOrDefault() == 0)
			{
                var BaseUrl = "http://localhost:5255/";
			//	SessionLineItemOptions LineItems;
				var options = new Stripe.Checkout.SessionCreateOptions()
                {
                    SuccessUrl = BaseUrl + $"customer/cart/OrderConfirm?id={cart.OrderHeader.Id}",
                    CancelUrl = BaseUrl + $"customer/cart/index",
                    LineItems = new List<SessionLineItemOptions>(),
                    Mode = "payment"
				};
                
                foreach(var item in cart.CartList)
                {
                    var sessionlineitems = new SessionLineItemOptions()
                    {
                        PriceData = new SessionLineItemPriceDataOptions()
                        {
                            UnitAmount = (long)(item.Price * 100),
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions() {
                                Name = item.product.Title
                            }
                        },
                        Quantity = item.Count
                    };
					options.LineItems.Add(sessionlineitems);
				}
                var service = new Stripe.Checkout.SessionService();
                Stripe.Checkout.Session session = service.Create(options);
                unitofwork.OrderHeaderRepository.updateStripeId(
                    cart.OrderHeader.Id,
                    session.Id,
                    session.PaymentIntentId
                    );
                unitofwork.save();
                Response.Headers.Add("Location", session.Url);
                return new StatusCodeResult(303);
			}


			return RedirectToAction("OrderConfirm", new {id = cart.OrderHeader.Id});
		}

        public ActionResult OrderConfirm(int id)
        {
            OrderHeader header = unitofwork.OrderHeaderRepository.Get(x => x.Id == id
            , includeProperties: "AppUser");
            if (header.OrderStatus != SD.PaymentDelayedDone)
            {
                var serice = new Stripe.Checkout.SessionService();
                Stripe.Checkout.Session session = serice.Get(header.SessionId);
                if (session.PaymentStatus.ToLower() == "paid")
                {
                    unitofwork.OrderHeaderRepository.updateStatus(id,
                        session.Id, session.PaymentIntentId);
                    unitofwork.OrderHeaderRepository.updateStripeId(
                        id, SD.StatusApproved, SD.PaymentDone);
                    unitofwork.save();
                }

                List<Cart> carts = unitofwork.CartRepository.GetAll(
                    x => x.CustomerId == header.UserId).ToList();
                unitofwork.CartRepository.RemoveRange(carts);
                unitofwork.save();

            }


            return View(id);
        }

		public ActionResult Plus(int Id)
        {
            var dbCartItem = unitofwork.CartRepository.Get(x=>x.Id == Id);
            dbCartItem.Count += 1;
            unitofwork.CartRepository.update(dbCartItem);
            unitofwork.save();
            return RedirectToAction("Index");
        }

        public ActionResult Minus(int Id)
        {
            var dbCartItem = unitofwork.CartRepository.Get(x => x.Id == Id);
            if (dbCartItem.Count <=1)
            {
                unitofwork.CartRepository.Remove(dbCartItem);
            }
            else
            {
                dbCartItem.Count -= 1;
                unitofwork.CartRepository.update(dbCartItem);
            }
            unitofwork.save();
            return RedirectToAction("Index");
        }
        public ActionResult Remove(int Id)
        {
                var dbCartItem = unitofwork.CartRepository.Get(x => x.Id == Id);
                unitofwork.CartRepository.Remove(dbCartItem);
                unitofwork.save();
                return RedirectToAction("Index");
        }



        private double CalculatePrice(Cart cart)
        {
            if (cart.Count <= 50)
            {
                return cart.product.Price;
            }
            else
            {
                if (cart.Count <= 100)
                {
                    return cart.product.Price50;
                }
                else
                {
                    return cart.product.Price100;
                }
            }

        }
    }
}
