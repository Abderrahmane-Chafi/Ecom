using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Models.ViewModels
{
    public class HomeVM
    {
        [ValidateNever]
       public IEnumerable<Brand> Brands { get; set; }
        [ValidateNever]
        public Product Product1 { get; set; }
        public Product Product2 { get; set; }
        public Product Product3 { get; set; }

        public Feedback Feedback { get; set; }
        [ValidateNever]
        public IEnumerable<Feedback> Feedbacks { get; set; }
    }
}
