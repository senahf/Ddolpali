﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Commands.Permissions.Levels;
using Discord.Modules;
using System.IO;

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

                group.CreateCommand("notification")
                    .Alias("notif")
                    .Description("PMs the user when their followed dramas have updates!")
                    .Parameter("keyword", ParameterType.Unparsed)
                    .Do(async e =>
                    {
                        if (string.IsNullOrWhiteSpace(e.GetArg("keyword")))
                        {
                            await e.Channel.SendMessage($"The correct syntax for the command is ```xl\n!notification keyword(s) here```");
                            return;
                        }
                        try
                        {
                            String entry = $"KD{e.GetArg("keyword")};{e.User.Id}";
                            if (File.ReadAllText("notifications.txt").Contains(entry))
                            {
                                await e.Channel.SendMessage($"{e.User.Mention}, You already have a notification for **{e.GetArg("keyword")}**");
                                return;
                            }

                            File.AppendAllText("notifications.txt", $"{entry}\r\n");
                            await e.User.SendMessage($"{e.User.Mention}: Added notification for `{e.GetArg("keyword")}`");
                            
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Notifications: {ex}");
                        }
                    });
            });
        }
    }
}
