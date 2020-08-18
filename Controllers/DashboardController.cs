using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TwitchLogs_Web.Models;

namespace TwitchLogs_Web.Controllers
{
    public class DashboardController : BaseController
    {
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(ILogger<DashboardController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        [Authorize]
        public IActionResult Index()
        {
            var user = GetUser();
            if (user == null)
            {
                return Unauthorized("User not found.");
            }
            var channels = user.UserChannels.Select(x => x.Channel).ToList();
            var groupLogs = channels.Select(x =>
            {
                return new KeyValuePair<string, int>(x.Name, Database.Logs.Count(l => l.Channel == x.Name));
            }).ToDictionary(x => x.Key, x => x.Value);
            var model = new DashboardViewModel
            {
                User = user,
                Channels = channels,
                LogCounts = groupLogs
            };
            return View(model);

        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
