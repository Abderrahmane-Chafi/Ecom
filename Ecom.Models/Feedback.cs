using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Ecom.Models
{
    public class Feedback
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        [DisplayName("Share your feedback !")]
        public string Comment { get; set; }
        [Display(Name = "User")]
        [Required]
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }
    }
}
