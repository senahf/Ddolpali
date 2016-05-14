using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Commands.Permissions.Levels;
using Discord.Modules;
using System.IO;
using System.Net;
using System.Drawing;
using System.Drawing.Imaging;

namespace aegyoBot.Modules.Public
{

    internal class FeedModule : IModule
    {
        private ModuleManager _manager;
        private DiscordClient _client;

        public void Install(ModuleManager manager)
        {
            _manager = manager;
            _client = manager.Client;

            manager.CreateCommands("", group =>
            {
                group.MinPermissions((int)PermissionLevel.ServerAdmin);

                group.CreateCommand("")
                .Alias("")
                .Parameter("url", ParameterType.Required)
                .Do(async e => {

                    await e.Channel.SendMessage("Feeds Updated!");
                });
            });
        }
    }
}
