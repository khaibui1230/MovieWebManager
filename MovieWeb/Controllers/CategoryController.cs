using Microsoft.AspNetCore.Mvc;
using MovieWeb.Data;
using MovieWeb.Models;

namespace MovieWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public CategoryController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            List<Category> objCategoryList = _dbContext.Categories.ToList();
            return View(objCategoryList);
        }

        // create a new
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category category)
        {
            //  check the fied input is  valid
            if (ModelState.IsValid)
            {
                _dbContext.Categories.Add(category);
                _dbContext.SaveChanges();
                return RedirectToAction("Index");  //  you  can chang the action  of index or controller  here
            }
            return View();
        }
    }
}
