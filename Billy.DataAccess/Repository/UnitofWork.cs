using Billy.DataAccess.Data;
using Billy.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billy.DataAccess.Repository
{
    public class UnitofWork : IUnitofWork
    {
        private readonly BulkyDB db;
        public ICategoryRepository CategoryRepository { get; private set; }
        public IProductRepository ProductRepository { get; private set; }
        public ICompanyRepository CompanyRepository { get; private set; }
        public IAppUserRepository AppUserRepository { get; private set; }
        public ICartRepository CartRepository { get; private set; }
        public IOrderDetailsRepository OrderDetailsRepository { get; private set; }
        public IOrderHeaderRepository  OrderHeaderRepository { get; private set; }

        public UnitofWork(BulkyDB _db) 
        {
            db = _db;
            OrderDetailsRepository = new OrderDetailsRepository(db); 
            OrderHeaderRepository = new OrderHeaderRepository(db);
            AppUserRepository = new UserRepository(db); 
            CategoryRepository = new CategoryRepository(db);
            ProductRepository = new ProductRepository(db);
            CompanyRepository = new CompanyRepository(db);
            CartRepository = new CartRepository(db) ;
        }
        public void save()
        {
            db.SaveChanges();
        }
    }
}
