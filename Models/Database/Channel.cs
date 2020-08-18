using System.Collections.Generic;

namespace TwitchLogs_Web.Models.Database
{
    public class Channel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<UserChannel> UserChannels { get; set; }
    }
}
