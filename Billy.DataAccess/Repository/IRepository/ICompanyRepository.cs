using Billy.Models;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow.CopyAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billy.DataAccess.Repository.IRepository
{
    public interface  ICompanyRepository: IRepository<Company>    
    {
        void update(Company product);
    }
}
