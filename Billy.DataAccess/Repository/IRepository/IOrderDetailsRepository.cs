using Billy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billy.DataAccess.Repository.IRepository
{

    public interface IOrderDetailsRepository : IRepository<OrderDetail>
    {
        void update(OrderDetail product);
    }
}
