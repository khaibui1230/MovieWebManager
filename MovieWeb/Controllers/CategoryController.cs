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
            if (category.Name == category.DisplayOrder.ToString())
            {
                //  if the name = =  the  display
                ModelState.AddModelError("Name", "The Display Order does  not match the Name");
            }
            //  check the fied input is  valid
            if (ModelState.IsValid)
            {
                _dbContext.Categories.Add(category);
                _dbContext.SaveChanges();
                TempData["success"] = "Category created successfully";
                return RedirectToAction("Index");  //  you  can chang the action  of index or controller  here
            }
            return View();
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            // load the database of Sql server
            //Category? categoryFromDb = _dbContext.Categories.FirstOrDefault(c => c.Id == id);
            //Category? categoryFromDb = _dbContext.Categories.Where(u => u.Id == id).FirstOrDefault();
            Category? categoryFromDb = _dbContext.Categories.Find(id);
            if (categoryFromDb == null)
            {
                return NotFound();
            }

            return View(categoryFromDb);
        }
        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (category.Name == category.DisplayOrder.ToString())
            {
                //  if the name = =  the  display
                ModelState.AddModelError("Name", "The Display Order does  not match the Name");
            }
            //  check the fied input is  valid
            if (ModelState.IsValid)
            {
                _dbContext.Categories.Update(category);
                _dbContext.SaveChanges();
                TempData["success"] = "Category Edited successfully";
                return RedirectToAction("Index");  //  you  can chang the action  of index or controller  here
            }
            return View();
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Category? categoryFromDb = _dbContext.Categories.Find(id);
            if (categoryFromDb == null)
            {
                return NotFound();
            }

            return View(categoryFromDb);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteDb(int? id)
        {
            Category? obj = _dbContext.Categories.Find(id);
            if (id == null)
            { return NotFound(); }

            _dbContext.Categories.Remove(obj); // delete the obj
            _dbContext.SaveChanges();
            TempData["success"] = "Category Deleted successfully";
            return RedirectToAction("Index");  //  you  can chang the action  of index or controller  here

            
        }
    }
}
