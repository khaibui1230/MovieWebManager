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
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext dbContxext;
        
        public CategoryRepository(ApplicationDbContext dbContxext) : base(dbContxext)
        {
            this.dbContxext = dbContxext;
        }
        public void Update(Category category)
        {
            dbContxext.Update(category);

        }
    }
}
