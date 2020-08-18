using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TwitchLib.Api;
using TwitchLogs_Web.Extensions;
using TwitchLogs_Web.Models;
using TwitchLogs_Web.Models.Database;

namespace TwitchLogs_Web.Controllers
{
    [Route("[controller]/[action]")]
    public class ChannelController : BaseController
    {
        [Authorize]
        [HttpGet("{channelName}")]
        public async Task<IActionResult> Add(string channelName)
        {
            var response = new StatusMessageModel();
            var user = GetUser();
            if (user == null)
            {
                return Unauthorized("User not found.");
            }
            if (user.IsLogging(channelName))
            {
                response.Message = "You are already logging this channel.";
            }
            else
            {
                //Check enough credits.
                if (user.Credits >= 1)
                {
                    var channel = Database.Channels.FirstOrDefault(x => x.Name == channelName);
                    if (channel == null)
                    {
                        //No channel found, so add it.
                        var api = new TwitchAPI();
                        api.Settings.ClientId = "vrb2zs5y4cc94vknoalfgxygzacfcw";
                        var userChannel = (await api.V5.Users.GetUsersByNameAsync(new List<string> { channelName })).Matches.FirstOrDefault();
                        if (userChannel == null)
                        {
                            response.Message = "This channel does not exist.";
                            return Json(response);
                        }
                        var newChannel = new Channel { Name = channelName };
                        Database.Channels.Add(newChannel);
                        Database.SaveChanges();
                        channel = newChannel;
                    }
                    //Add channel to user's logged channels.
                    Database.UserChannels.Add(new UserChannel { Channel = channel, User = user });
                    user.Credits--;
                    Database.SaveChanges();
                    response.Success = true;
                    response.Message = "Added channel to logs!";
                }
                else
                {
                    response.Message = "You don't have enough credits.";
                }
            }
            return Json(response);
        }

        [Authorize]
        [HttpGet("{channelId}")]
        public async Task<IActionResult> Remove(int channelId)
        {
            var response = new StatusMessageModel();
            var user = GetUser();
            if (user == null)
            {
                return Unauthorized("User not found.");
            }
            if (!user.IsLogging(channelId))
            {
                response.Message = "You are not currently logging this channel.";
            }
            else
            {
                var userChannel = user.UserChannels.FirstOrDefault(x => x.Channel.Id == channelId);
                if(userChannel == null)
                {
                    response.Message = "You are not currently logging this channel.";
                }
                else
                {
                    Database.UserChannels.Remove(userChannel);
                    await Database.SaveChangesAsync();
                    response.Success = true;
                    response.Message = "Channel was removed!";
                }
            }
            return Json(response);
        }
    }
}
