namespace TwitchLogs_Web.Models.Database
{
    public class UserChannel
    {
        public int Id { get; set; }

        public virtual User User { get; set; }
        public virtual Channel Channel { get; set; }
    }
}
