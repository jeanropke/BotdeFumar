using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client.Events;

namespace BotdeFumar.Core.Commands
{
    public class Game : CommandBase
    {
        public override void Run(OnChatCommandReceivedArgs e)
        {
            if (e.Command.ChatMessage.Username.ToLower() != "jean__")
                return;

            string game = "";
            using (WebClient client = new WebClient())
            {
                game = client.DownloadString($"https://decapi.me/twitch/game/{BotEnvironment.Settings["twitch.channel.name"]}");
            }

            using (WebClient client = new WebClient())
            {


                client.Encoding = Encoding.UTF8;
                client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                Console.WriteLine($"https://www.google.com.br/search?q={game}+wikipedia");
                string htmlCode = client.DownloadString($"https://www.google.com.br/search?q={game}");

                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(htmlCode);

                File.WriteAllText("debug.txt.html", htmlCode);
                HtmlNode node = doc.DocumentNode.SelectSingleNode("//div[contains(@class, 'hp-xpdbox')]");

                if (node != null)
                {
                    HtmlNodeCollection spans = node.SelectNodes(".//span");
                    if (spans[0] != null)
                    {
                        HtmlNode aNode = spans[0].SelectSingleNode(".//a");
                        if (aNode != null)
                        {
                            spans[0].RemoveChild(aNode);
                            //BotEnvironment.Bot.Client.SendMessage(BotEnvironment.Settings["twitch.channel.name"], spans[0].InnerText);

                        }
                    }
                }

            }
        }
    }
}
