using Movie.DataAccess.Data;
using Movie.DataAccess.Repository.IRepository;
using Movie.Models;

namespace Movie.DataAccess.Repository
{
    // Kế thừa phương thức từ lớp cha Repository với thuộc tính của Category
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CategoryRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public void Update(Category category)
        {
            _dbContext.Categories.Update(category);
        }
    }
}
