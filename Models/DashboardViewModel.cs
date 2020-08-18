using System.Collections.Generic;
using TwitchLogs_Web.Models.Database;

namespace TwitchLogs_Web.Models
{
    public class DashboardViewModel : BaseViewModel
    {
        public List<Channel> Channels { get; set; }
        public Dictionary<string, int> LogCounts { get; set; }
    }
}
