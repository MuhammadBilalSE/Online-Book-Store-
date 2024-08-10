using Billy.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billy.DataAccess.Repository.IRepository
{
    public interface IUnitofWork
    {
        ICategoryRepository CategoryRepository { get; }
        IProductRepository ProductRepository { get; }
        ICompanyRepository CompanyRepository { get ; }  
        IAppUserRepository  AppUserRepository { get; }
        ICartRepository CartRepository { get; }
        IOrderHeaderRepository OrderHeaderRepository { get; }
        IOrderDetailsRepository OrderDetailsRepository { get; }


        void save();
    }
}
