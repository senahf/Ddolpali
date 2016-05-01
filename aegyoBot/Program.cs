using System;
using System.Threading.Tasks;
using Discord.Modules;
using Discord.Commands;
using Discord.Audio;
using Discord;

namespace aegyoBot
{
    public class Program
    {
        public static void Main(string[] args) => new Program().Start(args);

        private const string AppName = "aegyoBot";
        private const string AppUrl = "https://github.com/senahf/aegyoBot";

        public static DiscordClient _client;

        private void Start(string[] args)
        {
            Console.WriteLine("Welcome to aegyoBot!");
            GlobalSettings.Load();

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
                // x.ErrorHandler [ChatterBotAPI? (aka CleverBot for Invalid commands]
            })
            .UsingModules()
            .UsingAudio(x =>
            {
                x.Mode = AudioMode.Outgoing;
                x.EnableMultiserver = true;
                x.EnableEncryption = true;
                x.Bitrate = AudioServiceConfig.MaxBitrate;
                x.BufferLength = 10000;
            });
            _client.MessageReceived += Client_MessageReceived;

            _client.ExecuteAndWait(async () =>
            {
                while (true)
                {
                    try
                    {
                        await _client.Connect(GlobalSettings.Discord.Token); // await _client.Connect(GlobalSettings.Discord.User, GlobalSettings.Discord.Pass);
                        _client.SetGame("#KDrama");
                        Console.WriteLine("Bot has been initialized!");
                        break;
                    } catch (Exception ex)
                    {
                        _client.Log.Error($"Login Failed", ex);
                        await Task.Delay(_client.Config.FailedReconnectDelay);
                    }
                }
            });
        }

        private static async void Client_MessageReceived(object sender, MessageEventArgs e)
        {
            
        }
    }
}
