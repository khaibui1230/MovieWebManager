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
    //[Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CompanyController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();

            return View(objCompanyList);
        }

        // create a new
        public IActionResult UpSert(int? id) // change to UpSert
        {


            if (id == null || id == 0)
            {
                return View(new Company()); // create a  Company
            }
            else
            {
                // update  the Company
                Company companyObj = _unitOfWork.Company.Get(u => u.Id == id);
                return View(companyObj);

            }

        }
        [HttpPost]
        public IActionResult UpSert(Company companyObj)
        {
            //  check the fied input is  valid
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;

                // add or update the db if the data is exist on 
                if (companyObj.Id == 0)
                {
                    _unitOfWork.Company.Add(companyObj);
                }
                else
                {
                    _unitOfWork.Company.Update(companyObj);
                }

                _unitOfWork.Save();
                TempData["success"] = "Company created successfully";
                return RedirectToAction("Index");  //  you  can chang the action  of index or controller  here
            }
            else
            {
                return View(companyObj);
            }
        }


        #region API Call
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();
            //return the new Json to get api
            return Json(new { data = objCompanyList });
        }

        // Delete func
        
        public IActionResult Delete(int? id)
        {
            var CompanyToBeDeleted = _unitOfWork.Company.Get(u => u.Id == id);
            if (CompanyToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleted" });
            };

            _unitOfWork.Company.Remove(CompanyToBeDeleted);
            _unitOfWork.Save();
            //return the new Json to get api
            return Json(new { success = true, message = "Deleted Successfull" });
        }
        #endregion
    }
}
