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
    public class OrderDetailsRepository : Repository<OrderDetail>, IOrderDetailsRepository
    {
        private readonly BulkyDB db;

        public OrderDetailsRepository(BulkyDB _db) : base(_db)
        {
            db = _db;
        }

        public void update(OrderDetail header)
        {
            db.OrderDetails.Update(header);
        }
    }
}
