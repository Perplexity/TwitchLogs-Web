namespace TwitchLogs_Web.Models.Database
{
    public class Log
    {
        public int Id { get; set; }
        public string Channel { get; set; }
        public string Sender { get; set; }
        public string Message { get; set; }
        public string Colour { get; set; }
        public long Timestamp { get; set; }
        public MessageType Type { get; set; }
    }
}
