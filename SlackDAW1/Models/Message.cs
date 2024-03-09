using System;
using System.ComponentModel.DataAnnotations;

namespace SlackDAW1.Models
{
    public class Message
    {
        [Key]
        public int MessageID { get; set; }

        [Required(ErrorMessage = "Message body is required")]
        [MinLength(1, ErrorMessage = "Minim 1 caracter")]
        public string Body { get; set; }

        [DataType(DataType.DateTime)]
        [Required]
        public DateTime Timestamp { get; set; }

        public string? SenderID { get; set; }
        public virtual ApplicationUser? Sender { get; set; }

        public int? ChannelID { get; set; }
        public virtual Channel? Channel { get; set; }
    }
}
