using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TwitchLib.Client.Events;

namespace BotdeFumar.Core.Commands
{
    public class PriceCommand : CommandBase
    {
        public override void Run(OnChatCommandReceivedArgs e)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Encoding = Encoding.UTF8;

                    //client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");

                    string args = e.Command.ArgumentsAsString;
                    //if (args == "maria renata")
                    //    args = "whore";


                    string htmlCode = client.DownloadString($"https://www.bing.com/search?q=site:store.steampowered.com+{args}");
                    GetPrice(htmlCode);

                    //BotEnvironment.Bot.Client.SendMessage(BotEnvironment.Settings["twitch.channel.name"], client.DownloadString($"http://openrdr.000webhostapp.com/price.php?q={args}"));


                }
            }
            catch (Exception ex)
            {
                BotEnvironment.Bot.Client.SendMessage(BotEnvironment.Settings["twitch.channel.name"], $"Deu erro com sua busca... Tenta de novo FeelsOkayMan");
                Logger.LogException(ex.ToString());
            }
        }

        private void GetPrice(string html)
        {
            string priceBRL = "free";
            string priceARS = "free";
            string gameName = "";
            bool alreadySend = false;
            HtmlDocument document = new HtmlDocument();

            document.LoadHtml(html);

            HtmlNodeCollection nodes = document.DocumentNode.SelectNodes("//a");

            foreach (HtmlNode node in nodes)
            {
                if (!alreadySend)
                {

                    if (node.OuterHtml.Contains("https://store.steampowered.com/app/"))
                    {
                        string url = node.OuterHtml;
                        Match match = Regex.Match(url, "https://store.steampowered.com/app/(.*)");

                        if (match.Success)
                        {
                            string appId = match.Groups[1].Value.Split('/')[0];

                            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                            using (WebClient client = new WebClient())
                            {
                                string gameContentBRL = client.DownloadString($"https://store.steampowered.com/api/appdetails?appids={appId}&cc=br");
                                string gameContentARS = client.DownloadString($"https://store.steampowered.com/api/appdetails?appids={appId}&cc=ar&filters=price_overview");

                                dynamic jsonBRL = JsonConvert.DeserializeObject(gameContentBRL);
                                dynamic jsonARS = JsonConvert.DeserializeObject(gameContentARS);

                                if (jsonBRL[appId].data.price_overview != null)
                                {
                                    priceBRL = jsonBRL[appId].data.price_overview.final_formatted;
                                    priceARS = jsonARS[appId].data.price_overview.final_formatted;
                                }

                                gameName = jsonBRL[appId].data.name;

                                if (gameName.Length > 0)
                                {
                                    if (priceBRL == "free")
                                    {
                                        BotEnvironment.Bot.Client.SendMessage(BotEnvironment.Settings["twitch.channel.name"], $"{gameName} não tem preço FeelsMan https://store.steampowered.com/app/{appId}");
                                    }
                                    else
                                    {
                                        BotEnvironment.Bot.Client.SendMessage(BotEnvironment.Settings["twitch.channel.name"], $"{gameName} custa {priceBRL.Replace(",", ".").Replace(" ", "")} ({priceARS.Replace(",", ".").Replace(" ", "")}) https://store.steampowered.com/app/{appId}");
                                    }
                                }
                                else
                                {
                                    BotEnvironment.Bot.Client.SendMessage(BotEnvironment.Settings["twitch.channel.name"], $"Não achei nada não FeelsBadMan");
                                }

                                alreadySend = true;
                            }
                        }
                    }
                }
            }            
        }

    }
}
