using Movie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movie.DataAccess.Repository.IRepository
{
    public interface IOrderHeaderRepository : IRepository<OrderHeader>
    {
        void Update (OrderHeader orderHeader);
        void UpdateStatus(int orderId,string orderStatus, string? paymentStatus = null);
        void UpdateStripePaymentID(int orderId, string sessionId ,string stripePaymentId);
    }
}
