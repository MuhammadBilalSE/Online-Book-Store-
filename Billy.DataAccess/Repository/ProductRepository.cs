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
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly BulkyDB db;
        public ProductRepository(BulkyDB _db) : base(_db) 
        {
            db = _db;
        }

        public void update(Product product)
        {
           db.Update(product);
        }
    }
}
