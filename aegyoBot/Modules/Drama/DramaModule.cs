using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Commands.Permissions.Levels;
using Discord.Modules;
using System.IO;
using System.Threading;

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
                        string keyword = e.GetArg("keyword").ToLower();
                        if (string.IsNullOrWhiteSpace(keyword))
                        {
                            await e.Channel.SendMessage($"The correct syntax for the command is ```xl\n!notification keyword(s) here```");
                            return;
                        }
                        try
                        {
                            String entry = $"KD{keyword};{e.User.Id}";
                            if (File.ReadAllText("notifications.txt").Contains(entry))
                            {
                                await e.Channel.SendMessage($"{e.User.Mention}, You already have a notification for **{keyword}**");
                                return;
                            }

                            File.AppendAllText("notifications.txt", $"{entry}\r\n");
                            await e.User.SendMessage($"{e.User.Mention}: Added notification for `{keyword}`");

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Notifications: {ex}");
                        }
                    });

                group.CreateCommand("dnotif") // todo: maybe have IDs instead for notifications?
                    .Alias("delnotif")
                    .Description("Deletes the specified notification!")
                    .Parameter("keyword", ParameterType.Unparsed)
                    .Do(async e =>
                    {
                        string keyword = e.GetArg("keyword").ToLower();
                        List<string> Entries = new List<string>();

                        try
                        {
                            using (StreamReader file = new StreamReader("notifications.txt"))
                            {
                                while (!file.EndOfStream)
                                {
                                    string line = file.ReadLine();
                                    Entries.Add(line);
                                }
                            }
                            String entry = $"KD{keyword};{e.User.Id}";
                            if (Entries.Contains(entry))
                            {
                                Entries.Remove(entry);
                                File.WriteAllLines("notifications.txt", Entries);
                                await e.Channel.SendMessage($"Deleted `{keyword}`");
                            } else
                            {
                                await e.Channel.SendMessage($"Keyword [{keyword}] does not exist!");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error getting notifs: " + ex);
                        }
                    });

                group.CreateCommand("notifs")
                .Alias("notifications")
                .Do(async e =>
                {
                    List<string> Entries = new List<string>();
                    try
                    {
                        using (StreamReader file = new StreamReader("notifications.txt"))
                        {
                            while (!file.EndOfStream)
                            {
                                string line = file.ReadLine();
                                int index = line.IndexOf(";");
                                string keyword = (index > 0 ? line.Substring(0, index) : "").Remove(0, 2).ToLower();
                                string user = line.Substring(line.LastIndexOf(';') + 1);
                                if (user == e.User.Id.ToString())
                                {
                                    Entries.Add(keyword);
                                }
                            }
                            
                            await e.Channel.SendMessage(String.Join(", ", Entries));
                        }
                    } catch (Exception ex)
                    {
                        Console.WriteLine("Error getting notifs: " + ex);
                    }
                });
            });
        }
    }
}
