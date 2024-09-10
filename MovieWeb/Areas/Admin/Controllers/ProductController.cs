using Microsoft.AspNetCore.Mvc;
using Movie.DataAccess.Data;
using Movie.DataAccess.Repository.IRepository;
using Movie.Models;


namespace MovieWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll().ToList();
            return View(objProductList);
        }

        // create a new
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Product objProduct)
        {
            //  check the fied input is  valid
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Add(objProduct);
                _unitOfWork.Save();
                TempData["success"] = "Product created successfully";
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
            //Product? ProductFromDb = _dbContext.Categories.FirstOrDefault(c => c.Id == id);
            //Product? ProductFromDb = _dbContext.Categories.Where(u => u.Id == id).FirstOrDefault();
            Product? ProductFromDb = _unitOfWork.Product.Get(u => u.Id == id);
            if (ProductFromDb == null)
            {
                return NotFound();
            }

            return View(ProductFromDb);
        }
        [HttpPost]
        public IActionResult Edit(Product objProduct)
        {
            //  check the fied input is  valid
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Update(objProduct);
                _unitOfWork.Save();
                TempData["success"] = "Product Edited successfully";
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
            Product? ProductFromDb = _unitOfWork.Product.Get(u => u.Id == id);
            if (ProductFromDb == null)
            {
                return NotFound();
            }

            return View(ProductFromDb);
        }
        [HttpPost, ActionName("Delete")] 

        public IActionResult DeleteDb(int? id)
        {
            Product? objProduct = _unitOfWork.Product.Get(u => u.Id == id);
            if (id == null)
            { return NotFound(); }

            _unitOfWork.Product.Remove(objProduct); // delete the obj
            _unitOfWork.Save();
            TempData["success"] = "Product Deleted successfully";
            return RedirectToAction("Index");  //  you  can chang the action  of index or controller  here


        }
    }
}
