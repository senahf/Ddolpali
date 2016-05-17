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
using System.Text;
using System.Collections;

namespace aegyoBot.Modules.Drama
{
    internal class DramaModule : IModule
    {
        private ModuleManager _manager;
        private DiscordClient _client;

        public static string Soundex(string data)
        {
            StringBuilder result = new StringBuilder();
            if (data != null && data.Length > 0)
            {
                string previousCode = "", currentCode = "", currentLetter = "";
                result.Append(data.Substring(0, 1));

                for (int i = 1; i < data.Length; i++)
                {
                    currentLetter = data.Substring(i, 1).ToLower();
                    currentCode = "";

                    if ("bfpv".IndexOf(currentLetter) > -1)
                        currentCode = "1";
                    else if ("cgjkqsxz".IndexOf(currentLetter) > -1)
                        currentCode = "2";
                    else if ("dt".IndexOf(currentLetter) > -1)
                        currentCode = "3";
                    else if (currentLetter == "l")
                        currentCode = "4";
                    else if ("mn".IndexOf(currentLetter) > -1)
                        currentCode = "5";
                    else if (currentLetter == "r")
                        currentCode = "6";

                    if (currentCode != previousCode)
                        result.Append(currentCode);

                    if (result.Length == 4) break;

                    if (currentCode != "")
                        previousCode = currentCode;
                }
            }
            if (result.Length < 4)
            {
                result.Append(new String('0', 4 - result.Length));
            }
            return result.ToString().ToUpper();
        }

        public static int Difference(string data1, string data2)
        {
            int result = 0;
            string soundex1 = Soundex(data1);
            string soundex2 = Soundex(data2);
            if (soundex1 == soundex2)
                result = 4;
            else
            {
                string sub1 = soundex1.Substring(1, 3);
                string sub2 = soundex1.Substring(2, 2);
                string sub3 = soundex1.Substring(1, 2);
                string sub4 = soundex1.Substring(1, 1);
                string sub5 = soundex1.Substring(2, 1);
                string sub6 = soundex1.Substring(3, 1);

                if (soundex2.IndexOf(sub1) > -1)
                    result = 3;
                else if (soundex2.IndexOf(sub2) > -1)
                    result = 2;
                else if (soundex2.IndexOf(sub3) > -1)
                    result = 2;
                else
                {
                    if (soundex2.IndexOf(sub4) > -1)
                        result++;
                    else if (soundex2.IndexOf(sub5) > -1)
                        result++;
                    else if (soundex2.IndexOf(sub6) > -1)
                        result++;
                }

                if (soundex1.Substring(0, 1) == soundex2.Substring(0, 1))
                    result++;
            }
            return (result == 0) ? 1 : result;
        }
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
                            if (!Entries.Any())
                            {
                                await e.Channel.SendMessage($"{e.User.Mention}, You don't have any notifications!");
                            } else
                                await e.Channel.SendMessage(String.Join(", ", Entries));
                        }
                    } catch (Exception ex)
                    {
                        Console.WriteLine("Error getting notifs: " + ex);
                    }
                });
                group.CreateCommand("diff")
                .Parameter("data1", ParameterType.Required)
                .Parameter("data2", ParameterType.Required)
                .Do(async e =>
                {
                    int diff = Difference(e.GetArg("data1"), e.GetArg("data2"));
                    await e.Channel.SendMessage($"Similarity: {diff}/4");
                });
            });
        }
    }
}
