using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billy.Models.ViewModels
{
    public class CartVM
    {
        public IEnumerable<Cart> CartList { get; set; }

        public OrderHeader OrderHeader { get; set; }    

    }
}
