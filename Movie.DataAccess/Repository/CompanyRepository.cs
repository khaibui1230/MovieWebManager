using Movie.DataAccess.Data;
using Movie.DataAccess.Repository.IRepository;
using Movie.Models;

namespace Movie.DataAccess.Repository
{
    // Kế thừa phương thức từ lớp cha Repository với thuộc tính của Category
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CompanyRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public void Update(Company company)
        {
            throw new NotImplementedException();
        }
    }
}
