using Movie.DataAccess.Data;
using Movie.DataAccess.Repository.IRepository;
using Movie.Models;

namespace Movie.DataAccess.Repository
{
    // Kế thừa phương thức từ lớp cha Repository với thuộc tính của Category
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ShoppingCartRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public void Update(ShoppingCart shoppingCart)
        {
            _dbContext.ShoppingCarts.Update(shoppingCart);
        }
    }
}
