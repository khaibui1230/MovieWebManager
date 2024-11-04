using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movie.DataAccess.Repository;
using Movie.DataAccess.Repository.IRepository;
using Movie.Models;
using Movie.Models.ViewModel;
using Movie.Untility;
using Stripe;
using Stripe.Checkout;

namespace MovieWeb.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class OrderController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    [BindProperty] public OrderVm OrderVm { get; set; }

    public OrderController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // GET
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Details(int orderId)
    {
        OrderVm = new()
        {
            OrderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == orderId, includesProperties: "ApplicationUser"),
            OrderDetails =
                _unitOfWork.OrderDetail.GetAll(u => u.OrderHearderId == orderId, includesProperties: "Product")
        };
        return View(OrderVm);
    }

    [Authorize(Roles = Sd.RoleAdmin + "," + Sd.RoleEmployee)]
    [HttpPost]
    public IActionResult UpdateOrderDetails()
    {
        var orderHeaderFromDb = _unitOfWork.OrderHeader.Get(u => u.Id == OrderVm.OrderHeader.Id);
        orderHeaderFromDb.Name = OrderVm.OrderHeader.Name;
        orderHeaderFromDb.PhoneNumber = OrderVm.OrderHeader.PhoneNumber;
        orderHeaderFromDb.StreetAddress = OrderVm.OrderHeader.StreetAddress;
        orderHeaderFromDb.City = OrderVm.OrderHeader.City;
        orderHeaderFromDb.State = OrderVm.OrderHeader.State;
        orderHeaderFromDb.PostalCode = OrderVm.OrderHeader.PostalCode;

        if (!string.IsNullOrEmpty(OrderVm.OrderHeader.Carrier))
        {
            orderHeaderFromDb.Carrier = OrderVm.OrderHeader.Carrier;
        }

        if (!string.IsNullOrEmpty(OrderVm.OrderHeader.TrackingNumber))
        {
            orderHeaderFromDb.TrackingNumber = OrderVm.OrderHeader.TrackingNumber;
        }

        _unitOfWork.OrderHeader.Update(orderHeaderFromDb);
        _unitOfWork.Save();

        TempData["Success"] = "Order has been updated";

        return RedirectToAction(nameof(Details), new { orderId = OrderVm.OrderHeader.Id });
    }

    [Authorize(Roles = Sd.RoleAdmin + "," + Sd.RoleEmployee)]
    [HttpPost]
    public IActionResult StartProcessing()
    {
        _unitOfWork.OrderHeader.UpdateStatus(OrderVm.OrderHeader.Id,Sd.StatusInProcess);
        _unitOfWork.Save();
        TempData["Success"] = "Order has been started";
        return RedirectToAction(nameof(Details), new { orderId = OrderVm.OrderHeader.Id });
    }
    
    [Authorize(Roles = Sd.RoleAdmin + "," + Sd.RoleEmployee)]
    [HttpPost]
    public IActionResult ShipOrder()
    {
        var orderHeaderFromDb = _unitOfWork.OrderHeader.Get(u => u.Id == OrderVm.OrderHeader.Id);
        orderHeaderFromDb.TrackingNumber = OrderVm.OrderHeader.TrackingNumber;
        orderHeaderFromDb.Carrier = OrderVm.OrderHeader.Carrier;
        orderHeaderFromDb.OrderStatus = Sd.StatusShipped;
        orderHeaderFromDb.ShippingDate = DateTime.Now;
         // if the conmpany user we can due the day to pay
         if (orderHeaderFromDb.PaymentStatus == Sd.PaymentStatusDelayedPayment)
         {
             orderHeaderFromDb.PaymentDueDate = DateOnly.FromDateTime(DateTime.Now.AddDays(30));
         }

         _unitOfWork.OrderHeader.Update(orderHeaderFromDb);
        _unitOfWork.Save();
        TempData["Success"] = "Order has been shipped";
        return RedirectToAction(nameof(Details), new { orderId = OrderVm.OrderHeader.Id });
    }

    [Authorize(Roles = Sd.RoleAdmin + "," + Sd.RoleEmployee)]  
    [HttpPost]
    public IActionResult CancelOrder()
    {
        var orderHeaderFromDb = _unitOfWork.OrderHeader.Get(u => u.Id == OrderVm.OrderHeader.Id);

        if (orderHeaderFromDb.PaymentStatus == Sd.StatusApproved)
        {
            var options = new RefundCreateOptions
            {
                Reason = RefundReasons.RequestedByCustomer,
                PaymentIntent = orderHeaderFromDb.PaymentIntenId,
            };
            
            var service = new RefundService();
            Refund refund = service.Create(options);
            
            _unitOfWork.OrderHeader.UpdateStatus(OrderVm.OrderHeader.Id, Sd.StatusCancelled,Sd.StatusRefunded);
        }
        else
        {
            _unitOfWork.OrderHeader.UpdateStatus(OrderVm.OrderHeader.Id, Sd.StatusCancelled, Sd.StatusCancelled);
        }
        
        _unitOfWork.Save();
        TempData["Success"] = "Order has been cancelled";
        Console.WriteLine("The order has been cancelled");
        return RedirectToAction(nameof(Details), new { orderId = OrderVm.OrderHeader.Id });
    }

    [ActionName("Details")]
    [HttpPost]
    public IActionResult Details_Pay_Now()
    {
        
        if (OrderVm == null || OrderVm.OrderHeader == null)
        {
            return BadRequest("Order information is missing.");
        }
        
        OrderVm.OrderHeader = _unitOfWork.OrderHeader
            .Get(u => u.Id == OrderVm.OrderHeader.Id, includesProperties: "ApplicationUser");
        OrderVm.OrderDetails = _unitOfWork.OrderDetail
            .GetAll(u => u.OrderHearderId == OrderVm.OrderHeader.Id, includesProperties: "Product");
        
        //stripe logic
        var domain = Request.Scheme + "://" + Request.Host.Value + "/";
        var options = new SessionCreateOptions()
        {
            SuccessUrl = domain + $"admin/order/PaymentConfirmation?orderHeaderId={OrderVm.OrderHeader.Id}",
            CancelUrl = domain + "admin/order/delete?orderId={OrderVm.OrderHeader.Id}",
            LineItems = new List<SessionLineItemOptions>(),
            Mode = "payment",
        };

        foreach (var item in OrderVm.OrderDetails)
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
        _unitOfWork.OrderHeader.UpdateStripePaymentID(OrderVm.OrderHeader.Id, session.Id, session.PaymentIntentId);
        _unitOfWork.Save();
        Response.Headers.Add("Location", session.Url);
        return new StatusCodeResult(303);

    }
    
    public IActionResult PaymentConfirmation(int orderHeaderId) {

        OrderHeader orderHeader = _unitOfWork.OrderHeader
            .Get(u => u.Id == orderHeaderId);
        if(orderHeader.PaymentStatus== Sd.PaymentStatusDelayedPayment) {
            //this is an order by company

            var service = new SessionService();
            Session session = service.Get(orderHeader.SectionId);

            if (session.PaymentStatus.ToLower() == "paid") {
                _unitOfWork.OrderHeader.UpdateStripePaymentID(orderHeaderId, session.Id, session.PaymentIntentId);
                _unitOfWork.OrderHeader.UpdateStatus(orderHeaderId,orderHeader.OrderStatus,  Sd.PaymentStatusApproved);
                _unitOfWork.Save();
            }
            //HttpContext.Session.Clear();

        }
        return View(orderHeaderId);
    }
 
    #region API Call

    [HttpGet]
    public IActionResult GetAll(string status)
    {
        Console.WriteLine($"Status received: {status}");

        IEnumerable<OrderHeader> objOrderHeaders;
        //Set the roles only the Admin  or Employee can see the All Order

        if (User.IsInRole(Sd.RoleAdmin) || User.IsInRole(Sd.RoleEmployee))
        {
            objOrderHeaders = _unitOfWork.OrderHeader.GetAll(includesProperties: "ApplicationUser");
        }
        else
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            objOrderHeaders = _unitOfWork.OrderHeader.GetAll(u => u.ApplicationUserId == userId,
                includesProperties: "ApplicationUser");
        }

        switch (status?.ToLower())
        {
            case "pending":
                objOrderHeaders = objOrderHeaders.Where(u => u.OrderStatus == Sd.StatusPending);
                break;
            case "inprocess":
                objOrderHeaders = objOrderHeaders.Where(u => u.OrderStatus == Sd.StatusInProcess);
                break;
            case "completed":
                objOrderHeaders = objOrderHeaders.Where(u => u.OrderStatus == Sd.StatusCompleted);
                break;
            case "approved":
                objOrderHeaders = objOrderHeaders.Where(u => u.OrderStatus == Sd.StatusApproved);
                break;
            default:
                break;
        }

        //return the new Json to get api
        return Json(new { data = objOrderHeaders });
    }

    #endregion
}