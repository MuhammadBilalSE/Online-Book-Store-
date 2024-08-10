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
    internal class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly BulkyDB db;
        public CategoryRepository(BulkyDB _db) : base(_db) 
        {
            db = _db;
        }
        public void save()
        {
            db.SaveChanges();
        }

        public void update(Category category)
        {
           db.Update(category);
        }
    }
}
