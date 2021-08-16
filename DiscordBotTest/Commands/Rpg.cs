using System;
using System.Threading.Tasks;
using Discord.Commands;
using System.Text.Json;
using System.IO;
using System.Xml.Serialization;
using DiscordBotTest.Models;
using System.Collections.Generic;
using System.Linq;

namespace DiscordBotTest.Commands
{
    public class Rpg : ModuleBase
    {

        public string pathTurnos = @"Memory/turnos.txt";

        public string pathPlayers = @"Memory/players.xml";
        [Command("d")]
        public async Task diceAsync(string request)
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

        [Command("o")]
        public async Task addRoundAsync(string request)
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

        [Command("N")]
        public async Task nextAsync()
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

        [Command("l")]
        public async Task lastAsync()
        {
            FileStream file = File.OpenRead(pathTurnos);
            var turno = await JsonSerializer.DeserializeAsync<Turnos>(file);
            turno.pos -= 1;
            if (turno.pos < 0) turno.pos = turno.ordem.Length -1;
            await Context.Channel.SendMessageAsync($" o ultimo a jogar foi {turno.ordem[turno.pos]}");
        }

        [Command("nw")]
        public async Task nowAsync()
        {
            FileStream file = File.OpenRead(pathTurnos);
            var turno = await JsonSerializer.DeserializeAsync<Turnos>(file);
            await Context.Channel.SendMessageAsync($" Turno de {turno.ordem[turno.pos]}");
        }
        [Command("s")]
        public  async Task savecharAsync(string request)
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
            if (File.Exists(pathPlayers))
            {
                Stream stream = File.Open(pathPlayers, FileMode.Open);
                jsonList = (List<Player>)serial.Deserialize(stream);
                stream.Close();
            }
            Stream str = File.OpenWrite(pathPlayers);
            
            jsonList.Add(Player);
            serial.Serialize(str,jsonList);

            str.Close();
            await Context.Channel.SendMessageAsync($"player {Player.nome} adicionado!");       
        }
    
        [Command("ch")]
        public async Task GetPlayerAsync(string request)
        {
            XmlSerializer serial = new XmlSerializer(typeof(List<Player>));
            List<Player> jsonList = new List<Player>();

            Stream stream = File.Open(pathPlayers, FileMode.Open);
            jsonList = (List<Player>)serial.Deserialize(stream);
            stream.Close();
            Player player = jsonList.Where(x=> x.nome.ToLower() == request.ToLower()).FirstOrDefault();
            if (player != null) await Context.Channel.SendMessageAsync($"player {player.nome} tem hp {player.hpAgora}/{player.HpCheio}!");
            else await Context.Channel.SendMessageAsync($"player não existe");
            
        }
    }

}
