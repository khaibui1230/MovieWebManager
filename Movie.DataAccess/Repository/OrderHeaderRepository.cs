using Movie.DataAccess.Data;
using Movie.DataAccess.Repository.IRepository;
using Movie.Models;

namespace Movie.DataAccess.Repository
{
    // Kế thừa phương thức từ lớp cha Repository với thuộc tính của OrderHeader
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public OrderHeaderRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public void Update(OrderHeader orderHeader)
        {
            _dbContext.OrderHeaders.Update(orderHeader);
        }


        public void UpdateStatus(int orderId, string orderStatus, string? paymentStatus = null)
        {
            var orderFromDb = _dbContext.OrderHeaders.FirstOrDefault(u => u.Id == orderId);
            if (orderFromDb != null)
            {
                orderFromDb.OrderStatus = orderStatus;
                if (!string.IsNullOrEmpty(paymentStatus))
                {
                    orderFromDb.PaymentStatus = paymentStatus;
                }
            }
        }

        public void UpdateStripePaymentID(int orderId, string sessionId, string stripePaymentId)
        {
            var orderFromDb = _dbContext.OrderHeaders.FirstOrDefault(u => u.Id == orderId);
            if (!string.IsNullOrEmpty(sessionId))
            {
                orderFromDb.SectionId = sessionId;
            }

            if (!string.IsNullOrEmpty(stripePaymentId))
            {
                orderFromDb.PaymentIntenId = stripePaymentId;
                orderFromDb.OrderDate = DateTime.Now;
            }
        }
    }
}