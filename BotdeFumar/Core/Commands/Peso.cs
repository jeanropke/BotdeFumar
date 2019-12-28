using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client.Events;
using Newtonsoft.Json;

namespace BotdeFumar.Core.Commands
{
    public class Peso : CommandBase
    {
        public override void Run(OnChatCommandReceivedArgs e)
        {
            string URL = "https://economia.awesomeapi.com.br/all/ARS-BRL";


            using (WebClient client = new WebClient())
            {
                string jsonContent = client.DownloadString(URL);
                dynamic json = JsonConvert.DeserializeObject(jsonContent);

                string price = json.ARS.bid.ToString().Replace(",", ".");

                float curPrice = float.Parse(price);
                if (float.TryParse(e.Command.ArgumentsAsString, out float curPeso))
                {
                    BotEnvironment.Bot.Client.SendMessage(BotEnvironment.Settings["twitch.channel.name"], $"AR$1.00 = R${Math.Round(curPrice, 2)}. AR${curPeso} é R${Math.Round(curPrice * curPeso, 2)}");
                }

            }
        }
    }
}
