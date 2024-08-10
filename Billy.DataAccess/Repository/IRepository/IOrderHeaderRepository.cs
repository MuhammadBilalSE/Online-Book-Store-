using Billy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billy.DataAccess.Repository.IRepository
{

    public interface IOrderHeaderRepository : IRepository<OrderHeader>
    {
        void update(OrderHeader product);
        void updateStatus(int id, string orderstatus, string? paymentstatus);
        void updateStripeId (int id, string sessionId, string paymentIntendId);
	}
}
