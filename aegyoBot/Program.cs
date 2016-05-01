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
            Console.WriteLine("Hello World!");
            Console.ReadLine();
        }
    }
}
