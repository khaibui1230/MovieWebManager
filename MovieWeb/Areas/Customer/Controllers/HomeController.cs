using Microsoft.AspNetCore.Mvc;
using Movie.DataAccess.Repository.IRepository;
using Movie.Models;
using System.Diagnostics;

namespace MovieWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            // get all the product to display along with property
            IEnumerable<Product> productsList = _unitOfWork.Product.GetAll(includesProperties: "Category");
            return View(productsList);
        }
        public IActionResult Detail(int productId)
        {
            // get all the product to display along with property
            Product product = _unitOfWork.Product.Get(u => u.Id == productId, includesProperties: "Category");
            return View(product);
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
