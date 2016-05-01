using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Commands.Permissions.Levels;
using Discord.Modules;

namespace aegyoBot.Modules.Drama
{
    internal class DramaModule : IModule
    {
        private ModuleManager _manager;
        private DiscordClient _client;

        public void Install(ModuleManager manager)
        {
            _manager = manager;
            _client = manager.Client;

            manager.CreateCommands("", group =>
            {
                group.MinPermissions((int)PermissionLevel.User);

                group.CreateCommand("rawr")
                    .Alias("")
                    .Do(async e =>
                    {
                        await e.Channel.SendMessage("");
                    });
            });
        }
    }
}
