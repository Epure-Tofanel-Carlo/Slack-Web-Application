using SlackDAW1.Models;
using System.ComponentModel.DataAnnotations;

namespace SlackDAW1.Models
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }

        [Required(ErrorMessage = "Category name is required")]
        [StringLength(20, ErrorMessage = "Category name cannot be longer than 20 characters")]
        [MinLength(3, ErrorMessage = "Category name must be at least 3 characters")]
        public String CategoryName { get; set; }

    }
}
