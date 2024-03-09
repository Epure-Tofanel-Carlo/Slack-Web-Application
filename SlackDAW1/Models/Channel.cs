using System.ComponentModel.DataAnnotations;
using SlackDAW1.Models;

namespace SlackDAW1.Models
{
    public class Channel
    {
        [Key]
        public int ChannelID { get; set; }

        [Required(ErrorMessage = "Channel name is required")]
        [StringLength(20, ErrorMessage = "Channel name cannot be longer than 20 characters")]
        [MinLength(1, ErrorMessage = "Channel name must be at least 1 character long")]
        public string ChannelName { get; set; }

        [Required(ErrorMessage = "Channel description is required")]
        [StringLength(200, ErrorMessage = "Channel description cannot be longer than 200 characters")]
        [MinLength(10, ErrorMessage = "Channel description must be at least 10 characters")]
        public string ChannelDescription { get; set; }

        [Required(ErrorMessage = "Category is required")]
		public int? CategoryID { get; set; }

		public virtual Category? Category { get; set; }
	}
}
