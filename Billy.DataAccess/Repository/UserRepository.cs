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
    public class UserRepository : Repository<AppUser>, IAppUserRepository
    {
        private readonly BulkyDB db;

        public UserRepository(BulkyDB _db) : base(_db)
        {
            db = _db;
        }

        public void update(Cart cart)
        {
            db.ShoppingCart.Update(cart);
        }
    }
}
