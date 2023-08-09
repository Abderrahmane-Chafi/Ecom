using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Models.ViewModels
{
    public class ProductVM
    {
        public Product product { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> CategoryList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> GenderList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> BrandList { get; set; }
        [ValidateNever]
        public IEnumerable<Sizes> sizes { get; set; }
        [ValidateNever]
        public IEnumerable<ProductDetails> productDetails { get; set; }
        [ValidateNever]
        public IEnumerable<Sizes> Othersizes { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> StatusList { get; set; }
    }
}
