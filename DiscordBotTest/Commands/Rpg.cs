using System;
using System.Threading.Tasks;
using Discord.Commands;
using System.Text.Json;
using System.IO;
using System.Xml.Serialization;
using DiscordBotTest.Models;
using System.Collections.Generic;

namespace DiscordBotTest.Commands
{
    public class Rpg : ModuleBase
    {

        public string pathTurnos = @"Memory/turnos.txt";

        public string pathPlayers = @"Memory/players.xml";
        [Command("Dado")]
        public async Task dado(string request)
        {
            var dado = request.Split("d");
            
            Random r = new Random();
            var rodadas = 0;
            while (rodadas < Convert.ToInt32(dado[0]))
            {
                await Context.Channel.SendMessageAsync($" Seu dado numero {rodadas + 1} tirou {r.Next(1,Convert.ToInt32(dado[1]))}");
                rodadas += 1;
            }
            
        }

        [Command("ordem")]
        public async Task addRodada(string request)
        {
            
            
            var turno = new Turnos
            {
                pos = 0,
                ordem = request.Split(',')
            };
            var json = JsonSerializer.Serialize(turno);
            File.CreateText(pathTurnos);
            await File.WriteAllTextAsync(pathTurnos,json);

            await Context.Channel.SendMessageAsync($"Turno de {turno.ordem[0]}");     
        }

        [Command("next")]
        public async Task next()
        {
            FileStream file = File.OpenRead(pathTurnos);
            var turno = await JsonSerializer.DeserializeAsync<Turnos>(file);
            turno.pos += 1;
            if (turno.pos > turno.ordem.Length - 1) turno.pos = 0;
            Console.WriteLine(turno.ordem);
            await Context.Channel.SendMessageAsync($" Turno de {turno.ordem[turno.pos]}");
            var json = JsonSerializer.Serialize(turno);
            await File.WriteAllTextAsync(pathTurnos, json);     
        }

        [Command("last")]
        public async Task last()
        {
            FileStream file = File.OpenRead(pathTurnos);
            var turno = await JsonSerializer.DeserializeAsync<Turnos>(file);
            turno.pos -= 1;
            if (turno.pos < 0) turno.pos = turno.ordem.Length -1;
            await Context.Channel.SendMessageAsync($" o ultimo a jogar foi {turno.ordem[turno.pos]}");
        }

        [Command("now")]
        public async Task now()
        {
            FileStream file = File.OpenRead(pathTurnos);
            var turno = await JsonSerializer.DeserializeAsync<Turnos>(file);
            await Context.Channel.SendMessageAsync($" Turno de {turno.ordem[turno.pos]}");
        }
        [Command("SaveChar")]
        public  async Task savechar(string request)
        {
            
            var commando = request.Split(',');
            
            var Player = new Player
            {
                nome = commando[0],
                hpAgora = Convert.ToInt32(commando[1]),
                HpCheio = Convert.ToInt32(commando[2])
            };
            
            

            XmlSerializer serial = new XmlSerializer(typeof(List<Player>));
            
            List<Player> jsonList = new List<Player>();
            Stream stream = File.OpenWrite(pathPlayers);
            jsonList = (List<Player>)serial.Deserialize(stream);
            jsonList.Add(Player);
            serial.Serialize(stream,jsonList);

            stream.Close();
            

            
            await Context.Channel.SendMessageAsync($"player {Player.nome} adicionado!");       
        }
    }

}
