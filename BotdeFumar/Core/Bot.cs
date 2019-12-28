using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Events;
using BotdeFumar.Core.Structs;
using System.Threading;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Media;
using System.IO;
using System.Text.RegularExpressions;
using BotdeFumar.Core.Commands;

namespace BotdeFumar.Core
{
    public class Bot
    {
        private TwitchClient client;

        public List<string> WeaponsAudios = new List<string>();
        public List<string> ScreamAudio = new List<string>();

        private Timer timer;

        public TwitchClient Client { get { return this.client; } }
        public Timer Timer { get { return this.timer; } }

        public Dictionary<string, CommandBase> Commands = new Dictionary<string, CommandBase>();
        public Dictionary<string, bool> CommandsEnabled = new Dictionary<string, bool>();
        public Dictionary<string, bool> CommandPermissionOverride = new Dictionary<string, bool>();

        private string[] PepesList = new string[] { "ConcernFroge", "monkaGun", "monkaChrist", "peepoSip", "peepoSleepo", "PepeComfy", "Pogplant", "peepoLove", "FeelsMan", "peepoPants", "FeelsOkayMan", "POGGERS", "peepoSmoke", "peepoFreeze", "PepeLaugh", "monkaT", "peepoSmart", "monkaHmm", "Pepega", "peepoEggplant", "HUGERSPLANT", "peepoLewd", "PepeHandsE", "PepePls", "PepoDance", "HACKERMANS", "FeelsHentai", "peepoRun", "peepoFatPlant", "RarePepe", "FeelsBirthdayMan", "FeelsBadMan", "FeelsGoodMan", "monkaS", "FeelsAmazingMan" };
        private int MessagesTotal = 69;
        private int MessagesPepes = 45;
        private int MessagesPepesTotal = 45;

        public Bot()
        {
            AddAudiosToList();
            AddCommands();
            //timer = new Timer(SendMessage_Callback, null, 0, int.Parse(BotEnvironment.Settings["bot.timer"]) * 60 * 1000);

            ConnectionCredentials credentials = new ConnectionCredentials(BotEnvironment.Settings["twitch.bot.name"], BotEnvironment.Settings["twitch.bot.auth"]);

            client = new TwitchClient();
            client.Initialize(credentials, BotEnvironment.Settings["twitch.channel.name"]);

            client.OnLog += Client_OnLog;

            //client.OnChatCommandReceived += Client_OnChatCommandReceived;
            //client.OnRaidNotification += Client_OnRaidNotification;
            client.OnMessageReceived += Client_OnMessageReceived;
            //client.OnJoinedChannel += Client_OnJoinedChannel;
            //client.OnLeftChannel += Client_OnLeftChannel;
            //client.OnUserTimedout += Client_OnUserTimedout;
            //client.OnUserBanned += Client_OnUserBanned;
            //client.OnDisconnected += Client_OnDisconnected;

            client.Connect();
        }

        void AddCommands()
        {
            if (File.Exists("commands.ini"))
            {
                foreach (string line in File.ReadAllLines("commands.ini"))
                {
                    if (!line.Contains("="))
                        continue;

                    if (line.StartsWith("#"))
                        continue;

                    string command = line.Split('=')[0];
                    bool enabled = line.Split('=')[1] == "true" ? true : false;

                    this.CommandsEnabled.Add(command, enabled);
                }
            }
            else
            {
                Logger.WriteLine("Arquivo 'commands.ini' não encontrado. Todos os comandos foram ativados", Color.Red);
            }

            Commands.Add("tiro", new Tiro());
            Commands.Add("!", new Uptime());
            Commands.Add("chicote", new Chicote());
            Commands.Add("scream", new Scream());
            Commands.Add("game", new Game());
            Commands.Add("p", new Permission());
            Commands.Add("version", new BotVersion());
            Commands.Add("taskkill", new TaskKill());
            Commands.Add("peso", new Peso());
            Commands.Add("real", new RealCommand());
            Commands.Add("magic", new MagicCommand());

            //Commands.Add("price", new PriceCommand());
            //Commands.Add("alturadaellens", new TestCommand());
            //Commands.Add("test", new TestCommand());

            CommandPermissionOverride.Add("tiro", false);
            CommandPermissionOverride.Add("!", false);
            CommandPermissionOverride.Add("chicote", false);
            CommandPermissionOverride.Add("scream", false);
            CommandPermissionOverride.Add("game", false);
        }

        void AddAudiosToList()
        {
            foreach (string file in Directory.GetFiles("Audios", "*.wav", SearchOption.AllDirectories))
            {
                WeaponsAudios.Add(file);
            }

            foreach (string file in Directory.GetFiles("Audios", "*.mp3", SearchOption.AllDirectories))
            {
                ScreamAudio.Add(file);
            }
        }
 
        public void Client_OnUserTimedout(object sender, OnUserTimedoutArgs e)
        {
            if(BotEnvironment.Settings["bot.gun.enabled"] == "true")
                BotEnvironment.PlayAudio(WeaponsAudios);
        }

        public void Client_OnUserBanned(object sender, OnUserBannedArgs e)
        {
            if (BotEnvironment.Settings["bot.gun.enabled"] == "true")
                BotEnvironment.PlayAudio(WeaponsAudios);
        }

        public void GetLive_SendMessage(bool IsForced)
        {

            if (IsForced)
            {
                Timer.Change(int.Parse(BotEnvironment.Settings["bot.timer"]) * 60 * 1000, int.Parse(BotEnvironment.Settings["bot.timer"]) * 60 * 1000);
                GetLive_SendMessage(false);
            }
            else
            {
                try
                {
                    using (WebClient channel = new WebClient())
                    {
                        string channelId = channel.DownloadString($"https://decapi.me/twitch/id/{BotEnvironment.Settings["twitch.channel.name"]}");
                        string URL = $"https://api.twitch.tv/kraken/streams/{channelId}?client_id=xxtir11piwi1oweqrcail2v2ay7ae4";

                        if (client.IsConnected)
                        {
                            using (WebClient wc = new WebClient())
                            {
                                wc.Headers.Add("Accept", "application/vnd.twitchtv.v5+json");
                                string json = wc.DownloadString(URL);
                                dynamic parse = JObject.Parse(json);
                                if (parse.stream != null)
                                {
                                    int SmokeTime = int.Parse(BotEnvironment.Settings["bot.smoke.time"]);
                                    string time = parse.stream.created_at;
                                    TimeSpan diff = DateTime.Now.ToUniversalTime() - DateTime.Parse(time);

                                    string hours = diff.Hours.ToString() + (diff.Hours > 1 ? BotEnvironment.Texts["text.bot.hours"] : BotEnvironment.Texts["text.bot.hour"]);
                                    string minutes = diff.Minutes.ToString() + (diff.Minutes > 1 ? BotEnvironment.Texts["text.bot.minutes"] : BotEnvironment.Texts["text.bot.minute"]);


                                    string formatedTime = diff.Hours > 0 ? hours : "";
                                    if (diff.Hours > 0 && diff.Minutes > 0)
                                        formatedTime += " e ";
                                    formatedTime += diff.Minutes > 0 ? minutes : "";

                                    if (diff.TotalMinutes >= 10)
                                    {
                                        client.SendMessage(BotEnvironment.Settings["twitch.channel.name"], $"{BotEnvironment.Texts["text.bot.smoker"].Replace("%time%", formatedTime).Replace("%count%", Math.Floor(diff.TotalMinutes / SmokeTime).ToString() + (Math.Floor(diff.TotalMinutes / SmokeTime) > 1 ? BotEnvironment.Texts["text.bot.cigarettes"] : BotEnvironment.Texts["text.bot.cigarette"]))}");
                                    }
                                    else
                                    {
                                        client.SendMessage(BotEnvironment.Settings["twitch.channel.name"], $"{BotEnvironment.Texts["text.bot.smoker.none"]}");
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex.ToString());
                }
            }
            
        
        }

        private void SendMessage_Callback(Object o)
        {
            //GetLive_SendMessage(false);
        }

        private void Client_OnLog(object sender, OnLogArgs e)
        {
            if(BotEnvironment.Settings["show.logs"] == "true")
                Logger.WriteLine($"{e.BotUsername} - {e.Data}", Color.MediumSeaGreen);

            if (BotEnvironment.Settings["save.logs"] == "true")
                Logger.SaveLog($"{e.BotUsername} - {e.Data}");

            

        }

        private void Client_OnRaidNotification(object sender, OnRaidNotificationArgs e)
        {
            if (BotEnvironment.Settings["show.debug"] == "true")
                Logger.WriteLine("Client_OnRaidNotification", Color.DarkCyan);
        }


        private void Client_OnLeftChannel(object sender, OnLeftChannelArgs e)
        {
            if (BotEnvironment.Settings["show.debug"] == "true")
                Logger.WriteLine("Client_OnLeftChannel", Color.DarkCyan);
        }

        private void Client_OnDisconnected(object sender, OnDisconnectedEventArgs e)
        {
            Thread.Sleep(5000);

            if (BotEnvironment.Settings["show.debug"] == "true")
                Logger.WriteLine("Client_OnDisconnected", Color.DarkCyan);

            Logger.WriteLine("O bot foi desconectado", Color.IndianRed);
            Logger.WriteLine($"Tentando reconectar...", Color.IndianRed);

            
            if (BotEnvironment.Settings["console.beep"] == "true")
                Console.Beep();

            BotEnvironment.Bot.Client.Reconnect();
             
        }

        private void Client_OnJoinedChannel(object sender, OnJoinedChannelArgs e)
        {
            if (BotEnvironment.Settings["show.debug"] == "true")
                Logger.WriteLine("Client_OnJoinedChannel", Color.DarkCyan);

            if(BotEnvironment.Settings["bot.smoke.onconnect"] == "true")
                GetLive_SendMessage(false);

            //client.SendMessage(BotEnvironment.Settings["twitch.channel.name"], $"!scream {((DateTime.Now.Hour + 1) * (DateTime.Now.Minute + 1) * Hasher.Hash("jean__")) / DateTime.Now.Day}");

            
        }

        private void Client_OnChatCommandReceived(object sender, OnChatCommandReceivedArgs e)
        {
            if (e.Command.ChatMessage.Username.ToLower() == "nightbot")
                return;

            if (this.CommandsEnabled.ContainsKey(e.Command.CommandText))
            {
                if (!this.CommandsEnabled[e.Command.CommandText])
                    return;
            }

            if (Commands.ContainsKey(e.Command.CommandText))
            {
                Commands[e.Command.CommandText].Run(e);
            }            
        }

        private void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            if (e.ChatMessage.Username.ToLower() == "nightbot")
                return;

            /*if (BotEnvironment.Settings["bot.smoke.hydrated"] == "true")
            {
                if (e.ChatMessage.Username.ToLower() == "stay_hydrated_bot")
                {
                    client.SendMessage(BotEnvironment.Settings["twitch.channel.name"], $"{BotEnvironment.Texts["text.bot.hydrated"]}");
                }
            }

            if (BotEnvironment.Settings["bot.smoke.salve"] == "true")
            {
                Regex regex = new Regex("(?=.*manda)(?=.*salve)");

                if (regex.IsMatch(e.ChatMessage.Message.ToLower()) || e.ChatMessage.Message.ToLower().StartsWith("salve"))
                {
                    client.SendMessage(BotEnvironment.Settings["twitch.channel.name"], $"!salve");
                }
            }*/

            this.CountPepesInMessages(e.ChatMessage.Message);
        }

        private void CountPepesInMessages(string message)
        {
            bool pepeCounted = false;
            foreach (string word in message.Split(' '))
            {

                if (PepesList.Contains(word))
                {
                    if (!pepeCounted)
                        MessagesPepes++;

                    MessagesPepesTotal++;
                    pepeCounted = true;
                }
            }

            MessagesTotal++;

            Logger.SaveLog($"MessagesTotal: {MessagesTotal} / MessagesPepes: {MessagesPepes} / MessagesPepesTotal: {MessagesPepesTotal} / Ratio ({(MessagesPepes * 100f) / MessagesTotal}%) / pepeCounted: {pepeCounted}");
            Logger.WriteLine($"MessagesTotal: {MessagesTotal} / MessagesPepes: {MessagesPepes} / MessagesPepesTotal: {MessagesPepesTotal} / Rate ({(MessagesPepes * 100f) / MessagesTotal}%) / pepeCounted: {pepeCounted}", Color.MediumPurple);
        }

    }
}
