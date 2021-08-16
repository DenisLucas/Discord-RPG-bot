using System;

namespace DiscordBotTest.Models
{
    [Serializable()]
    public class Player
    {
        public string nome { get; set; }
        public int hpAgora { get; set; }
        public int HpCheio { get; set; }

    }
}
