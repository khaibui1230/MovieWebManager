using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movie.DataAccess.Repository.IRepository;
using Movie.Models;
using Movie.Models.ViewModel;
using System.Security.Claims;
using Movie.Untility;
using Stripe.Checkout;

namespace MovieWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        [BindProperty] public ShoppingCartVm ShoppingCartVm { get; set; }

        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            ShoppingCartVm = new()
            {
                ShoppingCartsList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId,
                    includesProperties: "Product"),
                OrderHeader = new()
            };

            //the sum of price base on quantity
            foreach (var cart in ShoppingCartVm.ShoppingCartsList)
            {
                cart.Price = GetPriceBasedOnQuantity(cart);
                ShoppingCartVm.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            }

            return View(ShoppingCartVm);
        }

        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            ShoppingCartVm = new()
            {
                ShoppingCartsList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId,
                    includesProperties: "Product"),
                OrderHeader = new()
            };
            ShoppingCartVm.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);
            ShoppingCartVm.OrderHeader.Name = ShoppingCartVm.OrderHeader.ApplicationUser.Name;
            ShoppingCartVm.OrderHeader.PhoneNumber = ShoppingCartVm.OrderHeader.ApplicationUser.PhoneNumber;
            ShoppingCartVm.OrderHeader.StreetAddress = ShoppingCartVm.OrderHeader.ApplicationUser.StreetAddress;
            ShoppingCartVm.OrderHeader.City = ShoppingCartVm.OrderHeader.ApplicationUser.City;
            ShoppingCartVm.OrderHeader.State = ShoppingCartVm.OrderHeader.ApplicationUser.State.ToString();
            ShoppingCartVm.OrderHeader.PostalCode = ShoppingCartVm.OrderHeader.ApplicationUser.PostalCode;


            //the sum of price base on quantity
            foreach (var cart in ShoppingCartVm.ShoppingCartsList)
            {
                cart.Price = GetPriceBasedOnQuantity(cart);
                ShoppingCartVm.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            }

            return View(ShoppingCartVm);
        }

        [HttpPost]
        [ActionName("Summary")]
        public IActionResult SummaryPOST()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVm.ShoppingCartsList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId,
                includesProperties: "Product");

            ShoppingCartVm.OrderHeader.OrderDate = System.DateTime.Now;
            ShoppingCartVm.OrderHeader.ApplicationUserId = userId;

            ApplicationUser applicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);


            foreach (var cart in ShoppingCartVm.ShoppingCartsList)
            {
                cart.Price = GetPriceBasedOnQuantity(cart);
                ShoppingCartVm.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            }

            if (applicationUser.CompanyId.GetValueOrDefault() == 0)
            {
                //it is a regular customer 
                ShoppingCartVm.OrderHeader.PaymentStatus = Sd.PaymentStatusPending;
                ShoppingCartVm.OrderHeader.OrderStatus = Sd.StatusPending;
            }
            else
            {
                //it is a company user
                ShoppingCartVm.OrderHeader.PaymentStatus = Sd.PaymentStatusDelayedPayment;
                ShoppingCartVm.OrderHeader.OrderStatus = Sd.StatusApproved;
            }

            _unitOfWork.OrderHeader.Add(ShoppingCartVm.OrderHeader);
            _unitOfWork.Save();
            foreach (var cart in ShoppingCartVm.ShoppingCartsList)
            {
                OrderDetail orderDetail = new()
                {
                    ProductId = cart.ProductId,
                    OrderHearderId = ShoppingCartVm.OrderHeader.Id,
                    Price = cart.Price,
                    Count = cart.Count
                };
                _unitOfWork.OrderDetail.Add(orderDetail);
                _unitOfWork.Save();
            }

            if (applicationUser.CompanyId.GetValueOrDefault() == 0)
            {
                //it is a regular customer account, and we need to capture payment
                //stripe logic
                var domain = Request.Scheme + "://" + Request.Host.Value + "/";
                var options = new SessionCreateOptions
                {
                    SuccessUrl = domain + $"customer/cart/OrderConfirmation?id={ShoppingCartVm.OrderHeader.Id}",
                    CancelUrl = domain + "customer/cart/index",
                    LineItems = new List<SessionLineItemOptions>(),
                    Mode = "payment",
                };

                foreach (var item in ShoppingCartVm.ShoppingCartsList)
                {
                    var sessionLineItem = new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(item.Price * 100), // $20.50 => 2050
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.Product.Title
                            }
                        },
                        Quantity = item.Count
                    };
                    options.LineItems.Add(sessionLineItem);
                }


                var service = new SessionService();
                Session session = service.Create(options);
                _unitOfWork.OrderHeader.UpdateStripePaymentID(ShoppingCartVm.OrderHeader.Id, session.Id,
                    session.PaymentIntentId);
                _unitOfWork.Save();
                Response.Headers.Add("Location", session.Url);
                return new StatusCodeResult(303);
            }

            return RedirectToAction(nameof(OrderConfirmation), new { id = ShoppingCartVm.OrderHeader.Id });
        }

        public IActionResult OrderConfirmation(int id)
        {
            OrderHeader orderHeader =
                _unitOfWork.OrderHeader.Get(u => u.Id == id, includesProperties: "ApplicationUser");
            if (orderHeader.PaymentStatus != Sd.PaymentStatusDelayedPayment)
            {
                //this is an order by customer

                var service = new SessionService();
                Session session = service.Get(orderHeader.SectionId);

                if (session.PaymentStatus.ToLower() == "paid")
                {
                    _unitOfWork.OrderHeader.UpdateStripePaymentID(id, session.Id, session.PaymentIntentId);
                    _unitOfWork.OrderHeader.UpdateStatus(id, Sd.StatusApproved, Sd.PaymentStatusApproved);
                    _unitOfWork.Save();
                }
                //HttpContext.Session.Clear();
            }

            List<ShoppingCart> shoppingCarts = _unitOfWork.ShoppingCart
                .GetAll(u => u.ApplicationUserId == orderHeader.ApplicationUserId).ToList();

            _unitOfWork.ShoppingCart.RemoveRange(shoppingCarts);
            _unitOfWork.Save();

            return View(id);
        }


        public IActionResult Plus(int cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);
            cartFromDb.Count += 1;
            _unitOfWork.ShoppingCart.Update(cartFromDb);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Minus(int cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);
            if (cartFromDb.Count <= 1)
            {
                //Remove the number of session
                HttpContext.Session.SetInt32(Sd.SessionCart,
                    _unitOfWork.ShoppingCart.GetAll(u=>u.ApplicationUserId == cartFromDb.ApplicationUserId).Count()-1);
                //remove that from cart
                _unitOfWork.ShoppingCart.Remove(cartFromDb);
            }
            else
            {
                cartFromDb.Count -= 1;
                _unitOfWork.ShoppingCart.Update(cartFromDb);
            }

            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(int cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);
            //remove that from cart
            _unitOfWork.ShoppingCart.Remove(cartFromDb);
            HttpContext.Session.SetInt32(Sd.SessionCart,
                _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == cartFromDb.ApplicationUserId).Count()-1);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public double GetPriceBasedOnQuantity(ShoppingCart shoppingCart)
        {
            if (shoppingCart.Count <= 50)
            {
                return shoppingCart.Product.Price;
            }
            else
            {
                if (shoppingCart.Count <= 100)
                {
                    return shoppingCart.Product.Price50;
                }
                else
                {
                    return shoppingCart.Product.Price100;
                }
            }
        }
    }
}