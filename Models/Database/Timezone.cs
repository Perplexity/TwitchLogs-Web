using System.Collections.Generic;

namespace TwitchLogs_Web.Models.Database
{
    public class Timezone
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
