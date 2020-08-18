using System.Collections.Generic;
using TwitchLogs_Web.Models.Database;

namespace TwitchLogs_Web.Models
{
    public class UserSettingsViewModel : BaseViewModel
    {
        public List<Timezone> Timezones { get; set; }
    }
}
