using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Movie.DataAccess.Repository.IRepository;
using Movie.Untility;

namespace MovieWeb.ViewComponent;

public class ShoppingCartViewComponent : Microsoft.AspNetCore.Mvc.ViewComponent
{
    private readonly IUnitOfWork _unitOfWork;

    public ShoppingCartViewComponent(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        //check the Role of user
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

        // Check the carrt in the db
        if (claims != null)
        {
            //check the data in the cart
            if (HttpContext.Session.GetInt32(Sd.SessionCart) == null)
            {
                HttpContext.Session.SetInt32(Sd.SessionCart,
                    _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claims.Value).Count());
            }

            return View(HttpContext.Session.GetInt32(Sd.SessionCart));
        }
        else
        {
            // clear the session cart
            HttpContext.Session.Clear();
            return View(0);
        }
    }
}