using Billy.DataAccess.Data;
using Billy.DataAccess.Repository.IRepository;
using Billy.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Billy.DataAccess.Repository
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private readonly BulkyDB db;

        public OrderHeaderRepository(BulkyDB _db) : base(_db)
        {
            db = _db;
        }

        public void update(OrderHeader header)
        {
            db.OrderHeaders.Update(header);
        }

		public void updateStatus(int id, string orderstatus, string? paymentstatus)
		{
			var orderheaderdb = db.OrderHeaders.FirstOrDefault(x => x.Id == id);
            if (orderheaderdb !=null)
            {
                orderheaderdb.OrderStatus = orderstatus;
                if (paymentstatus !=null)
                {
                    orderheaderdb.PaymentStatus = paymentstatus;
                }
            }
            
		}

		public void updateStripeId(int id, string sessionId, string paymentIntendId)
		{
			var dborderheader = db.OrderHeaders.FirstOrDefault(x=>x.Id == id);
            if (sessionId != null)
            {
                dborderheader.SessionId = sessionId;
            }
			if (paymentIntendId != null)
			{
				dborderheader.PaymentIntentId = paymentIntendId;
                dborderheader.PaymentDate = DateTime.Now;
			}
		}
	}
}
