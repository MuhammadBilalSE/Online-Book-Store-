using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Billy.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [ValidateNever]
        [Display(Name = "Category Name")]
        [MaxLength(30)]
        public string Name { get; set; }
        [Display(Name= "Display Order")]
        [Range(1,100, ErrorMessage ="Between 1 and 100 Please")]
        public int DisplayOrder { get; set; }

    }
}
