using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movie.Models.ViewModel
{
    public class ShoppingCartVm

    {
        public IEnumerable<ShoppingCart>? ShoppingCartsList { get; set; }
        public OrderHeader OrderHeader { get; set; }
    }
}
