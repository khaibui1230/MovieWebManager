using Movie.DataAccess.Data;
using Movie.DataAccess.Repository.IRepository;
using Movie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movie.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;

        public IApplicationUserRepository ApplicationUser { get; }
        public ICategoryRepository Category { get; private set; }
        public IOrderDetailRepository OrderDetail { get; private set; }
        public IOrderHeaderRepository OrderHeader { get; private set; }

        public IProductRepository Product {  get; private set; }

        public ICompanyRepository Company { get; private set; }
        public IShoppingCartRepository ShoppingCart { get; private set; }

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            ApplicationUser = new ApplicationUserRepository(_dbContext);
            Category = new CategoryRepository(_dbContext);
            Company = new CompanyRepository(_dbContext);
            OrderHeader = new OrderHeaderRepository(_dbContext);
            OrderDetail = new OrderDetailRepository(_dbContext);
            Product = new ProductRepository(_dbContext);
            ShoppingCart = new ShoppingCartRepository(_dbContext);
        }
        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}
