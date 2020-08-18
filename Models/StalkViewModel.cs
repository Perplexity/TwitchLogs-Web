using System.Collections.Generic;

namespace TwitchLogs_Web.Models
{
    public class StalkViewModel : BaseViewModel
    {
        public List<string> Channels { get; set; }
        public string Sender { get; set; }
        public string Timezone => User.Timezone.Name;
    }
}
