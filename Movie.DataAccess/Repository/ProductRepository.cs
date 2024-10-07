using Movie.DataAccess.Data;
using Movie.DataAccess.Repository.IRepository;
using Movie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Movie.DataAccess.Repository
{
    // ke thua phuong thuc tu lop cha Repository voi thuoc tinh cua Category
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext dbContxext;

        public ProductRepository(ApplicationDbContext dbContxext) : base(dbContxext)
        {
            this.dbContxext = dbContxext;
        }


        public void Update(Product product)
        {
            // anh xa the information from Db
            var objFromDb = dbContxext.Products.FirstOrDefault(u => u.Id == product.Id);
            if (objFromDb != null)
            {
                objFromDb.Title = product.Title;
                objFromDb.ISBN = product.ISBN;
                objFromDb.Price = product.Price;
                objFromDb.ListPrice = product.ListPrice;
                objFromDb.Price50 = product.Price50;
                objFromDb.Price100 = product.Price100;
                objFromDb.Description = product.Description;
                objFromDb.cateGoryId = product.cateGoryId;
                objFromDb.Author = product.Author;

                // just update the img if u choose the new url
                if (product.ImageUrl !=null)
                {
                    objFromDb.ImageUrl = product.ImageUrl;
                }
            }

        }
    }
}
