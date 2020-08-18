namespace TwitchLogs_Web.Models
{
    public class ViewLogsViewModel : BaseViewModel
    {
        public string Channel { get; set; }
        public string Sender { get; set; }
        public string Timezone => User.Timezone.Name;
    }
}
