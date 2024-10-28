using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Movie.DataAccess.Data;
using Movie.DataAccess.Repository.IRepository;
using Movie.Models;
using Movie.Untility;


namespace MovieWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles =SD.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Category> objCategoryList = _unitOfWork.Category.GetAll().ToList();
            return View(objCategoryList);
        }

        // create a new
        [Authorize(Roles = Sd.RoleAdmin)] 
        public IActionResult Create()
        {
            IEnumerable<SelectListItem> categoryList = _unitOfWork.Category
                .GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
            //hien thog ti 
            ViewBag.CategoryList = categoryList;
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
                _unitOfWork.Category.Add(category);
                _unitOfWork.Save();
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
            Category? categoryFromDb = _unitOfWork.Category.Get(u => u.Id == id);
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
                _unitOfWork.Category.Update(category);
                _unitOfWork.Save();
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
            Category? categoryFromDb = _unitOfWork.Category.Get(u => u.Id == id);
            if (categoryFromDb == null)
            {
                return NotFound();
            }

            return View(categoryFromDb);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteDb(int? id)
        {
            Category? obj = _unitOfWork.Category.Get(u => u.Id == id);
            if (id == null)
            { return NotFound(); }

            _unitOfWork.Category.Remove(obj); // delete the obj
            _unitOfWork.Save();
            TempData["success"] = "Category Deleted successfully";
            return RedirectToAction("Index");  //  you  can chang the action  of index or controller  here


        }
    }
}
