using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace Ecom.Models.ViewModels
{
    public class ProductsVM
    {
        public IEnumerable<Product> products { get; set; }
        public IEnumerable<Brand>  brands { get; set; }
        public IEnumerable<Category> categories { get; set; }

        public double Price { get; set; }
        public IPagedList<Product> PagedProducts { get; set; }
        public IEnumerable<string> SelectedBrandFilters { get; set; }
        public IEnumerable<string> SelectedCategoriesFilters { get; set; }



    }
}
