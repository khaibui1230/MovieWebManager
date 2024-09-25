using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Movie.DataAccess.Data;
using Movie.DataAccess.Repository.IRepository;
using Movie.Models;
using Movie.Models.ViewModels;


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
        public IActionResult UpSert(int? id) // change to UpSert
        {
            //// lay danh sach tu co so du lieu
            //ViewBag.CategoryList = CategoyList;
            ProductVM productVm = new()
            {
                CategoryList = _unitOfWork.Category.GetAll()
                .Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),

                Product = new Product()
            };

            if (id == null || id == 0)
            {
                return View(productVm); // create a  product
            }
            else
            {
                // update  the product
               productVm.Product = _unitOfWork.Product.Get(u => u.Id == id);
                return View(productVm);

            }

        }
        [HttpPost]
        public IActionResult UpSert(ProductVM obj, IFormFile file_img)
        {
            //  check the fied input is  valid
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Add(obj.Product);
                _unitOfWork.Save();
                TempData["success"] = "Product created successfully";
                return RedirectToAction("Index");  //  you  can chang the action  of index or controller  here
            }
            else
            {
                obj.CategoryList = _unitOfWork.Category.GetAll()
            .Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });

                return View(obj);
            }
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
