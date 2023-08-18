using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Models.ViewModels
{
    public class SizeVM
    {
        public Sizes size { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> SizeTypes { get; set; }
    }
}
