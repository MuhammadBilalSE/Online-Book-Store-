using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Billy.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        [ValidateNever]
        public Product product { get; set; }
        [Range(1,100, ErrorMessage = "Max 100 Products can be Shipped")]
        public int Count { get; set; }
        public string CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        [ValidateNever]
        public AppUser User { get; set; }
        [NotMapped]
        public double Price { get; set; }




    }
}
