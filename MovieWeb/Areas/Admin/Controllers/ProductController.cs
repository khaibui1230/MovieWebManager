using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Movie.DataAccess.Data;
using Movie.DataAccess.Repository.IRepository;
using Movie.Models;
using Movie.Models.ViewModels;
using Movie.Untility;


namespace MovieWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll(includesProperties: "Category").ToList();

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
        public IActionResult UpSert(ProductVM productVm, IFormFile? file_img)
        {
            //  check the fied input is  valid
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file_img != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file_img.FileName); // new file name
                    string productPath = Path.Combine(wwwRootPath, @"Images\Product");

                    // check the oldImg in exsist on the model
                    if (!string.IsNullOrEmpty(productVm.Product.ImageUrl))
                    {
                        // delete Old Img  
                        var oldImgPath =
                            Path.Combine(wwwRootPath, productVm.Product.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImgPath))
                        {
                            System.IO.File.Delete(oldImgPath);
                        }
                    }
                    //copy file 
                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file_img.CopyTo(fileStream);
                    }
                    productVm.Product.ImageUrl = @"\Images\Product\" + fileName;
                }
                //else
                //{
                //    // Nếu không có ảnh mới, giữ lại giá trị ImageUrl cũ từ cơ sở dữ liệu
                //    var objFromDb = _unitOfWork.Product.Update()
                //    productVm.Product.ImageUrl = objFromDb?.ImageUrl;
                //}

                // add or update the db if the data is exist on 
                if (productVm.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(productVm.Product);
                }
                else
                {
                    _unitOfWork.Product.Update(productVm.Product);
                }

                _unitOfWork.Save();
                TempData["success"] = "Product created successfully";
                return RedirectToAction("Index");  //  you  can chang the action  of index or controller  here
            }
            else
            {
                productVm.CategoryList = _unitOfWork.Category.GetAll()
            .Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });

                return View(productVm);
            }
        }


        #region API Call
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll(includesProperties: "Category").ToList();
            //return the new Json to get api
            return Json(new { data = objProductList });
        }

        // Delete func
        
        public IActionResult Delete(int? id)
        {
            var productToBeDeleted = _unitOfWork.Product.Get(u => u.Id == id);
            if (productToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleted" });
            };

            var oldImagePath =
                Path.Combine(_webHostEnvironment.WebRootPath,
                productToBeDeleted.ImageUrl.TrimStart('\\'));

            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }
            _unitOfWork.Product.Remove(productToBeDeleted);
            _unitOfWork.Save();
            //return the new Json to get api
            return Json(new { success = true, message = "Deleted Successfull" });
        }
        #endregion
    }
}
