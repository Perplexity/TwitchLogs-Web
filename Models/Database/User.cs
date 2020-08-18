using System.Collections.Generic;

namespace TwitchLogs_Web.Models.Database
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Credits { get; set; }

        public virtual Timezone Timezone { get; set; }
        public virtual ICollection<UserChannel> UserChannels { get; set; }
    }
}
