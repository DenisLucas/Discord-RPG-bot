using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace DiscordBotTest
{
    class Program
    {

        public static async Task Main(string[] args)
        => await Startup.RunAsync(args);

    }
}
