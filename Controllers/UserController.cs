using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using TwitchLib.Api.Core.Models.Undocumented.ChannelPanels;
using TwitchLib.Api.V5;
using TwitchLogs_Web.Models;
using TwitchLogs_Web.Models.Database;

namespace TwitchLogs_Web.Controllers
{
    public class UserController : BaseController
    {
        [HttpGet]
        public IActionResult Login()
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return View();
            }
            return RedirectToAction("Index", "Dashboard");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                await HttpContext.SignOutAsync();
            }
            return RedirectToAction("Login");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel login, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = Database.Users.FirstOrDefault(x => x.Username == login.Username && x.Password == login.Password);
                if (user == null)
                {
                    ModelState.AddModelError("Login", "Invalid username/password combination.");
                    return View();
                }
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
                identity.AddClaim(new Claim(ClaimTypes.Name, user.Username));
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                return RedirectToAction("Index", "Dashboard");
            }
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Update([FromBody] dynamic data)
        {
            var user = GetUser();
            if (user == null)
            {
                return Unauthorized("User not found.");
            }
            var settings = JObject.Parse(data.ToString());
            var response = new StatusMessageModel();
            if (settings.ContainsKey("timezone"))
            {
                if (int.TryParse(settings["timezone"].ToString(), out int timezoneId))
                {
                    var timezone = Database.Timezones.FirstOrDefault(x => x.Id == timezoneId);
                    if (timezone != null)
                    {
                        user.Timezone = timezone;
                    }
                }
            }
            await Database.SaveChangesAsync();
            response.Success = true;
            response.Message = "Settings have been saved!";
            return Json(response);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Settings()
        {
            var user = GetUser();
            if (user == null)
            {
                return Unauthorized("User not found.");
            }
            var model = new UserSettingsViewModel
            {
                User = user,
                Timezones = Database.Timezones.ToList()
            };
            return View(model);
        }
    }
}