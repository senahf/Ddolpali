using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Commands.Permissions.Levels;
using Discord.Modules;

namespace aegyoBot.Modules.Admin
{

    internal class ServerModule : IModule
    {
        private ModuleManager _manager;
        private DiscordClient _client;
        public void Install(ModuleManager manager)
        {
            _manager = manager;
            _client = manager.Client;

            manager.CreateCommands("", group =>
            {
                group.CreateCommand("setname")
                    .Description("Sets the name of the bot")
                    .Parameter("New Name", ParameterType.Unparsed)
                    .MinPermissions((int)PermissionLevel.BotOwner)
                    .Do(async e =>
                    {
                        await _client.CurrentUser.Edit(GlobalSettings.Discord.Token, e.GetArg("New Name"));
                        Console.WriteLine($"Name set to {e.Args[0]}");
                    });

                group.CreateCommand("setgame")
                    .Description("Sets the game playing to the specified argument")
                    .Parameter("Game")
                    .MinPermissions((int)PermissionLevel.BotOwner)
                    .Do(async e =>
                    {
                        _client.SetGame(e.Args[0]);
                        await e.Channel.SendMessage($"Set game status to `{e.Args[0]}`!");
                    });

                group.CreateCommand("prune")
                    .Description("Clears a number of messages from the channel.")
                    .Parameter("number")
                    .MinPermissions((int)PermissionLevel.ChannelModerator)
                    .Do(async e =>
                    {
                        int val;
                        if (string.IsNullOrWhiteSpace(e.GetArg("number")) || !int.TryParse(e.GetArg("number"), out val) || val < 0)
                            return;
                        foreach (var msg in await e.Channel.DownloadMessages(val))
                        {
                            await msg.Delete();
                            await Task.Delay(100);
                        }
                    });
            });
        }
    }
}
