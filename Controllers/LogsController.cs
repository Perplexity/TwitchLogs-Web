using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TwitchLogs_Web.Extensions;
using TwitchLogs_Web.Models;
using TwitchLogs_Web.Models.Database;

namespace TwitchLogs_Web.Controllers
{
    [Route("[controller]/[action]")]
    public class LogsController : BaseController
    {
        [HttpGet("{channel}/{sender?}")]
        [Authorize]
        public IActionResult View(string channel, string sender)
        {
            var user = GetUser();
            if (user == null)
            {
                return Unauthorized("User not found.");
            }
            if (user.IsLogging(channel))
            {
                return View(new ViewLogsViewModel{User = user, Channel = channel, Sender = sender});
            }
            return Unauthorized("You are not logging this channel.");
        }

        [HttpGet("{sender}")]
        [Authorize]
        public IActionResult Stalk(string sender)
        {
            var user = GetUser();
            if (user == null)
            {
                return Unauthorized("User not found.");
            }
            var logs = Database.Logs.Where(x => x.Sender == sender).ToList();
            logs = logs.Where(x => user.UserChannels.Any(y => y.Channel.Name == x.Channel)).ToList();
            var groupedLogs = logs.GroupBy(x => x.Channel).ToList();
            var model = new StalkViewModel
            {
                User = user,
                Channels = groupedLogs.Select(x => x.Key).ToList(),
                Sender = sender
            };
            return View(model);
        }

        [HttpGet("{channel}/{sender?}")]
        public IActionResult Get(string channel, string sender)
        {
            var from = long.MinValue;
            var to = long.MaxValue;
            var fromString = HttpContext.Request.Query["from"].ToString();
            var toString = HttpContext.Request.Query["to"].ToString();
            if(!string.IsNullOrEmpty(fromString))
            {
                from = long.Parse(fromString);
            }
            if (!string.IsNullOrEmpty(toString))
            {
                to = long.Parse(toString);
            }
            var logs = new List<Log>();
            if (IsAuthorized())
            {
                var user = GetUser();
                if (user != null)
                {
                    if (user.IsLogging(channel))
                    {
                        logs = GetLogs(channel, sender, from, to);
                    }
                }
            }
            return Json(new {Data = logs});
        }

        private List<Log> GetLogs(string channel, string sender, long from = long.MinValue, long to = long.MaxValue)
        {
            var logs = Database.Logs.Where(x => x.Channel == channel).ToList();
            if (sender != null)
            {
                logs = logs.Where(x => x.Sender.ToLower() == sender.ToLower()).ToList();
            }
            logs = logs.Where(x => x.Timestamp >= from && x.Timestamp <= to).ToList();
            return logs;
        }
    }
}
