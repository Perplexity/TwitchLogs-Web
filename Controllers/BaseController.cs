using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TwitchLogs_Web.Contexts;
using TwitchLogs_Web.Models.Database;

namespace TwitchLogs_Web.Controllers
{
    public class BaseController : Controller
    {
        protected TwitchContext Database { get; set; }

        public BaseController()
        {
            Database = new TwitchContext();
        }

        protected bool IsAuthorized()
        {
            return HttpContext.User.Identity.IsAuthenticated;
        }

        protected User GetUser()
        {
            var identity = HttpContext.User.Identity;
            return Database.Users.FirstOrDefault(x => x.Username == identity.Name);
        }

        protected override void Dispose(bool disposing)
        {
            Database.Dispose();
            base.Dispose(disposing);
        }
    }
}