using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Ecom.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public double Price { get; set; }      
        [ValidateNever]
        public string ImageUrl { get; set; }
        [ValidateNever]
        public string ImageUrl1 { get; set; }
        public int Quantity { get; set; } = 1;
        public string status { get; set; } = "Available";
        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        [ValidateNever]
        public Category Category { get; set; }
        [Required]
        [Display(Name = "Brand")]
        public int BrandId { get; set; }
        [ValidateNever]
        public Brand Brand { get; set; }
    }
}
