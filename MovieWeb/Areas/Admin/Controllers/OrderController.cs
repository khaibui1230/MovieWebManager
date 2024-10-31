using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movie.DataAccess.Repository;
using Movie.DataAccess.Repository.IRepository;
using Movie.Models;
using Movie.Models.ViewModel;
using Movie.Untility;

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