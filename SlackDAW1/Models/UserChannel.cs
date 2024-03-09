namespace SlackDAW1.Models
{
    public class UserChannel
    {
        public string UserID { get; set; }
        public virtual ApplicationUser User { get; set; }

        public int ChannelID { get; set; }
        public virtual Channel Channel { get; set; }

        public bool IsModerator { get; set; }
    }

}
