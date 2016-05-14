using System;
using System.Threading.Tasks;
using Discord.Modules;
using Discord.Commands;
using Discord.Audio;
using Discord;
using aegyoBot.Services;
using aegyoBot.Modules.Public;
using aegyoBot.Modules.Drama;
using aegyoBot.Modules.Admin;
using Discord.Commands.Permissions.Levels;
using System.Threading;
using System.IO;

namespace aegyoBot
{
    public class Program
    {
        public static void Main(string[] args) => new Program().Start(args);

        private const string AppName = "aegyoBot";
        private const string AppUrl = "https://github.com/senahf/aegyoBot";

        public static DiscordClient _client;
        public static DiscordClient _client2;
        public static DiscordClient _client3;

        private void Start(string[] args)
        {
            Console.WriteLine($"Welcome to {AppName}");
            /* Song Joong Ki*/
            GlobalSettings.Load();
            _client2 = new DiscordClient(x =>
            {
                x.AppName = AppName;
                x.AppUrl = AppUrl;
                x.MessageCacheSize = 0;
                x.UsePermissionsCache = true;
                x.EnablePreUpdateEvents = true;
            })
            .UsingCommands(x =>
             {
                 x.AllowMentionPrefix = false;
                 x.PrefixChar = Convert.ToChar("!");
                 x.HelpMode = HelpMode.Public;
                 // x.ExecuteHandler
                 // x.ErrorHandler [ChatterBotAPI? (aka CleverBot for Invalid commands)]
             })
            .UsingModules()
            .UsingPermissionLevels(PermissionResolver)
            .UsingAudio(x =>
            {
                x.Mode = AudioMode.Outgoing;
                x.EnableMultiserver = true;
                x.EnableEncryption = true;
                x.Bitrate = AudioServiceConfig.MaxBitrate;
                x.BufferLength = 10000;
            });
            _client2.AddModule<ServerModule>("Server", ModuleFilter.None);
            _client2.AddModule<PublicModule>("Public", ModuleFilter.None);
            _client2.AddModule<GreetModule>("Greet", ModuleFilter.None);

            _client2.ExecuteAndWait(async () =>
             {
                 while (true)
                 {
                     try
                     {
                         await _client2.Connect(GlobalSettings.Discord.Token2); // await _client.Connect(GlobalSettings.Discord.User, GlobalSettings.Discord.Pass);
                         _client2.SetGame("Welcome");
                         Console.WriteLine("Song Joong Ki has been initialized! (Welcome Bot)");
                         break;
                     }
                     catch (Exception ex)
                     {
                         Console.WriteLine($"Login Failed {ex}");
                         await Task.Delay(_client2.Config.FailedReconnectDelay);
                     }
                 }
                 /* MAIN BOT (Park Shin Hye)*/
                 _client = new DiscordClient(x =>
                 {
                     x.AppName = AppName;
                     x.AppUrl = AppUrl;
                     x.MessageCacheSize = 0;
                     x.UsePermissionsCache = true;
                     x.EnablePreUpdateEvents = true;
                 })
            .UsingCommands(x =>
            {
                x.AllowMentionPrefix = false;
                x.PrefixChar = Convert.ToChar(">");
                x.HelpMode = HelpMode.Public;
                // x.ExecuteHandler
                // x.ErrorHandler [ChatterBotAPI? (aka CleverBot for Invalid commands)]
            })
            .UsingModules()
            .UsingPermissionLevels(PermissionResolver)
            .UsingAudio(x =>
            {
                x.Mode = AudioMode.Outgoing;
                x.EnableMultiserver = true;
                x.EnableEncryption = true;
                x.Bitrate = AudioServiceConfig.MaxBitrate;
                x.BufferLength = 10000;
            });
                 _client.MessageReceived += Client_MessageReceived;
                 _client.AddService<SettingsService>();
                 _client.AddService<HttpService>();
                 _client.AddModule<PublicModule>("Public", ModuleFilter.None);
                 _client.AddModule<DramaModule>("Drama", ModuleFilter.None);
                 _client.AddModule<ServerModule>("Server", ModuleFilter.None);
                 _client.AddModule<ColorModule>("Color", ModuleFilter.None);

                 _client.ExecuteAndWait(async () =>
                 {
                     while (true)
                     {
                         try
                         {
                             await _client.Connect(GlobalSettings.Discord.Token); // await _client.Connect(GlobalSettings.Discord.User, GlobalSettings.Discord.Pass);
                             _client.SetGame("Notifications");
                             Console.WriteLine("Park Shin Hye has been initialized! (Notifications)");
                             break;
                         }
                         catch (Exception ex)
                         {
                             Console.WriteLine($"Login Failed {ex}");
                             await Task.Delay(_client.Config.FailedReconnectDelay);
                         }
                     }
                     _client3 = new DiscordClient(x =>
                     {
                         x.AppName = AppName;
                         x.AppUrl = AppUrl;
                         x.MessageCacheSize = 0;
                         x.UsePermissionsCache = true;
                         x.EnablePreUpdateEvents = true;
                     })
            .UsingCommands(x =>
            {
                x.AllowMentionPrefix = false;
                x.PrefixChar = Convert.ToChar("~");
                x.HelpMode = HelpMode.Public;
                // x.ExecuteHandler
                // x.ErrorHandler [ChatterBotAPI? (aka CleverBot for Invalid commands)]
            })
            .UsingModules()
            .UsingPermissionLevels(PermissionResolver)
            .UsingAudio(x =>
            {
                x.Mode = AudioMode.Outgoing;
                x.EnableMultiserver = true;
                x.EnableEncryption = true;
                x.Bitrate = AudioServiceConfig.MaxBitrate;
                x.BufferLength = 10000;
            });
                     _client3.AddService<SettingsService>();
                     _client3.AddService<HttpService>();
                     _client3.AddModule<PublicModule>("Public", ModuleFilter.None);
                     _client3.AddModule<ServerModule>("Server", ModuleFilter.None);
                     _client3.AddModule<FeedModule>("Feeds", ModuleFilter.None);

                     _client3.ExecuteAndWait(async () =>
                     {
                         while (true)
                         {
                             try
                             {
                                 await _client3.Connect(GlobalSettings.Discord.Token3); // await _client.Connect(GlobalSettings.Discord.User, GlobalSettings.Discord.Pass);
                                 _client3.SetGame("Placeholder");
                                 Console.WriteLine("Gong Hyo Jin has been initialized! (Twitch)");
                                 break;
                             }
                             catch (Exception ex)
                             {
                                 Console.WriteLine($"Login Failed {ex}");
                                 await Task.Delay(_client3.Config.FailedReconnectDelay);
                             }
                         }
                     });
                 });
             });
        }
        private static async void Client_MessageReceived(object sender, MessageEventArgs e)
        {
            // TODO: Twitch emote? Kakao emote?
            // TODO: Notifcations command
            if (e.Message.Text.StartsWith(">") || e.Message.Text.StartsWith(".") || e.Message.Text.StartsWith("~") || e.Channel == null || e.User.Id == _client.CurrentUser.Id || e.Server == null) return; // || e.User.Id == client.CurrentUser.Id
            new Thread(async () =>
            {
                try
                {
                    Thread.Sleep(2000);
                    Thread.CurrentThread.IsBackground = true;
                    using (StreamReader file = new StreamReader("notifications.txt"))
                    {
                        //string[] result = file.ReadToEnd().Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        //foreach (string line in result)
                        while (!file.EndOfStream)
                        {
                            string line = file.ReadLine();
                            int index = line.IndexOf(";");
                            string keyword = (index > 0 ? line.Substring(0, index) : "");
                            string user = line.Substring(line.LastIndexOf(';') + 1);
                            keyword = keyword.Remove(0, 2).ToLower();
                            if (e.Message.Text.ToLower().Contains(keyword))
                            {
                                if (Convert.ToUInt64(user) == e.User.Id) return;
                                Channel rawr = await _client.CreatePrivateChannel(Convert.ToUInt64(user));
                                await rawr.SendMessage($"{e.User.Name} mentioned you [{keyword}] in #{e.Channel.Name} with the following message:{Environment.NewLine}```xl{Environment.NewLine}{e.Message.Text}```");
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }).Start();
        }

        private int PermissionResolver(User user, Channel channel)
        {
            if (user.Id == GlobalSettings.users.owner || user.Id == GlobalSettings.users.owner)
                return (int)PermissionLevel.BotOwner;
            if (user.Server != null)
            {
                if (user == channel.Server.Owner)
                    return (int)PermissionLevel.ServerOwner;

                var serverPerms = user.ServerPermissions;
                if (serverPerms.ManageRoles)
                    return (int)PermissionLevel.ServerAdmin;
                if (serverPerms.ManageMessages && serverPerms.KickMembers && serverPerms.BanMembers)
                    return (int)PermissionLevel.ServerModerator;

                var channelPerms = user.GetPermissions(channel);
                if (channelPerms.ManagePermissions)
                    return (int)PermissionLevel.ChannelAdmin;
                if (channelPerms.ManageMessages)
                    return (int)PermissionLevel.ChannelModerator;
            }
            return (int)PermissionLevel.User;
        }
    }
}
