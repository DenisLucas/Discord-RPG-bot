using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace DiscordBotTest.Commands
{
    public class Geral : ModuleBase
    {
        public string[] ordem;
        public int pos = 0;
        [Command("ping")]
        public async Task Pong()
        {
            await Context.Channel.SendMessageAsync("pong");
        }

        [Command("ajuda")]
        public async Task Ajuda()
        {
            var builder = new EmbedBuilder()
                .WithTitle("Sua mãe")
                .WithDescription("Se vira!");
            var embed = builder.Build();
            await Context.Channel.SendMessageAsync("Mensagem de sua mãe",false,embed); 
        }

    }
}
