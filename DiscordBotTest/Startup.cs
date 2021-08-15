using System;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBotTest.services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DiscordBotTest
{
    public class Startup
    {
        public IConfigurationRoot configuration {get;}

        public Startup(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddYamlFile("_config.yml");

            configuration = builder.Build();
        }

        public static async Task RunAsync(string[] args)
        {
            var Startup = new Startup(args);
            await Startup.RunAsync();
        }

        public async Task RunAsync()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);

            var provider = services.BuildServiceProvider();
            provider.GetRequiredService<CommandHandler>();

            await provider.GetRequiredService<StartupServices>().StartAsync();
            await Task.Delay(-1);
        }
        private void ConfigureServices(IServiceCollection services)
        {
                services
                    .AddSingleton(new DiscordSocketClient(new DiscordSocketConfig
                {
                    LogLevel = Discord.LogSeverity.Verbose,
                    MessageCacheSize = 1000
                }))
                    .AddSingleton(new CommandService (new CommandServiceConfig
                    {
                        LogLevel = Discord.LogSeverity.Verbose,
                        DefaultRunMode = RunMode.Async,
                        CaseSensitiveCommands = false
                    }))
                    .AddSingleton<CommandHandler>()
                    .AddSingleton<StartupServices>()
                    .AddSingleton(configuration);
        }
    }
}
