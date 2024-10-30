using Microsoft.AspNetCore.Mvc;
using Movie.DataAccess.Repository;
using Movie.DataAccess.Repository.IRepository;
using Movie.Models;

namespace MovieWeb.Areas.Admin.Controllers;
[Area("admin")]
public class OrderController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public OrderController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    // GET
    public IActionResult Index()
    {
        return View();
    }
    
    #region API Call
    [HttpGet]
    public IActionResult GetAll()
    {
        List<OrderHeader> objOrderHeaders = _unitOfWork.OrderHeader.GetAll(includesProperties: "ApplicationUser").ToList();
        //return the new Json to get api
        return Json(new { data = objOrderHeaders });
    }

    #endregion
}