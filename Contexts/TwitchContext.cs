using Microsoft.EntityFrameworkCore;
using TwitchLogs_Web.Models.Database;

namespace TwitchLogs_Web.Contexts
{
    public class TwitchContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Channel> Channels { get; set; }
        public DbSet<UserChannel> UserChannels { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Timezone> Timezones { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies().UseMySql("server=localhost;database=twitchlogs;user=root;password=xxx");
        }
    }
}
