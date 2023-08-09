using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Models.ViewModels
{
    public class DetailsVM
    {
      public ShoppingCart cartObj { get; set; }
        public IEnumerable<ProductDetails> productDetails { get; set; }
    }
}
