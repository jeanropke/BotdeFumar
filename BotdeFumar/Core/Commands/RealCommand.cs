using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client.Events;

namespace BotdeFumar.Core.Commands
{
    public class RealCommand : CommandBase
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
                if (float.TryParse(e.Command.ArgumentsAsString, out float curReal))
                {

                    BotEnvironment.Bot.Client.SendMessage(BotEnvironment.Settings["twitch.channel.name"], $"R$1.00 = ARS${Math.Round(1f / curPrice, 2)}. R${curReal} é ARS${Math.Round(1f / curPrice * curReal, 2)}");
                }

            }
        }
    }
}
