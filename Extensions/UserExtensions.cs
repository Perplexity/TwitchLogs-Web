using System.Linq;
using TwitchLogs_Web.Models.Database;

namespace TwitchLogs_Web.Extensions
{
    public static class UserExtensions
    {
        public static bool IsLogging(this User user, string channel)
        {
            return user.UserChannels.Any(x => x.Channel.Name == channel);
        }
        public static bool IsLogging(this User user, int id)
        {
            return user.UserChannels.Any(x => x.Channel.Id == id);
        }
    }
}
