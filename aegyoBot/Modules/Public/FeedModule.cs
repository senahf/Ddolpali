using Discord;
using Discord.Commands.Permissions.Levels;
using Discord.Modules;
using CloudFlareUtilities;
using System.Net.Http;
using HtmlAgilityPack;
using System.IO;
using System.Linq;
using System;

namespace aegyoBot.Modules.Drama
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
                group.MinPermissions((int)PermissionLevel.User);

                group.CreateCommand("test")
                .Do(async e =>
                {
                    try
                    {


                        var handler = new ClearanceHandler();
                        var client = new HttpClient(handler);
                        File.AppendAllText("kissasian.html", await client.GetStringAsync("http://kissasian.com/"));
                        HtmlDocument doc = new HtmlDocument();
                        doc.Load("kissasian.html");
                        var root = doc.DocumentNode;
                        var a = root.Descendants()
                            .Where(n => n.GetAttributeValue("class", "").Equals("barContent"))
                            .Single()
                            .Descendants("a")
                            .Single();
                        var content = a.InnerText;
                        await e.Channel.SendMessage($"{a}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                });
            });
        }
    }
}
