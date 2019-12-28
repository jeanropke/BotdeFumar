using BotdeFumar.Core.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;

namespace BotdeFumar.Core
{
    public static class ConsoleCommand
    {
        public static string ReadLine()
        {
            Stream inputStream = Console.OpenStandardInput(1024);
            byte[] bytes = new byte[1024];
            int outputLength = inputStream.Read(bytes, 0, 1024);
            //Console.WriteLine(outputLength);
            char[] chars = Encoding.UTF7.GetChars(bytes, 0, outputLength);
            return new string(chars).Remove(outputLength-1);
        }

        public static IEnumerable<string> SpliceText(this string @this, int lineLength)
        {
            var currentString = string.Empty;
            var currentWord = string.Empty;

            foreach (var c in @this)
            {
                if (char.IsWhiteSpace(c))
                {
                    if (currentString.Length + currentWord.Length > lineLength)
                    {
                        yield return currentString;
                        currentString = string.Empty;
                    }
                    currentString += c + currentWord;
                    currentWord = string.Empty;
                    continue;
                }
                currentWord += c;
            };
            // The loop might have exited without flushing the last string and word
            yield return currentString;
            yield return currentWord;
        }

        public async static void InvokeCommand(string Command)
        {
            if (string.IsNullOrEmpty(Command))
                return;

            try
            {
                string[] Parameters = Command.Split(' ');

                switch (Parameters[0].ToLower())
                {


                    case "commands":
                        Logger.WriteLine("Comandos disponíveis no chat", Color.Cyan);

                        foreach(KeyValuePair<string, CommandBase> entry in BotEnvironment.Bot.Commands)
                        {
                            bool isEnabled = BotEnvironment.Bot.CommandsEnabled.ContainsKey(entry.Key) ? BotEnvironment.Bot.CommandsEnabled[entry.Key] : false;
                            Logger.WriteLine($"  {entry.Key} - { isEnabled }", Color.Cyan);
                        }

                        break;

                    case "refresh":
                        Logger.WriteLine("Recarregando arquivos de configurações e textos...", Color.Cyan);
                        BotEnvironment.LoadSettings();
                        BotEnvironment.LoadTexts();
                        Logger.WriteLine("Configurações atualizadas", Color.LightGreen);
                        break;

                    case "uptime":
                        TimeSpan uptime = DateTime.Now - Process.GetCurrentProcess().StartTime;
                        Logger.WriteLine($"Bot está ativo há {uptime.Days} dias, {uptime.Hours} horas e {uptime.Minutes} minutos", Color.Cyan);
                        break;

                    //case "dev":
                    //    BotEnvironment.Bot.Client.SendRaw("NOTICE #beringelamilgrau :a");
                    //    break;

                    case "clear":
                        Console.Clear();
                        Logger.WriteLine($"Console limpo!", Color.LightGreen);
                        break;

                    case "force":
                        BotEnvironment.Bot.GetLive_SendMessage(true);
                        break;

                    case "reconnect":
                        if (BotEnvironment.Bot.Client.IsConnected)
                        {
                            Logger.WriteLine($"Bot já está conectado!", Color.IndianRed);
                        }
                        else
                        {
                            Logger.WriteLine($"Tentando reconectar...", Color.IndianRed);
                            BotEnvironment.Bot.Client.Reconnect();
                        }

                        break;

                    case "copypasta":
                        if (BotEnvironment.Bot.Client.IsConnected)
                        {
                            BotEnvironment.Bot.Client.SendMessage(BotEnvironment.Settings["twitch.channel.name"], "FeelsMan");
                            if (File.Exists("copypasta.txt"))
                            {
                                Console.WriteLine("Copypasta desativado FeelsMan");
                                //int count = 1;
                                //foreach(string line in File.ReadAllLines("copypasta.txt"))
                                //{

                                //    await Task.Delay(6000);                                
                                //    BotEnvironment.Bot.Client.SendMessage(BotEnvironment.Settings["twitch.channel.name"], line);
                                //    BotEnvironment.Bot.Client.SendMessage(BotEnvironment.Settings["twitch.channel.name"], $"!addquote quote #{count++}");
                                //}

                                //foreach (string sendMessage in message.SpliceText(125))
                                //{
                                //    BotEnvironment.Bot.Client.SendMessage(BotEnvironment.Settings["twitch.channel.name"], sendMessage);
                                //ThreadPool.QueueUserWorkItem(new WaitCallback(delegate
                                //{
                                //    Thread.Sleep(2500);
                                //    Console.WriteLine(sendMessage);

                                //}));
                                //}
                            }
                        }
                        
                        break;

                    case "talk":
                        if (BotEnvironment.Bot == null)
                        {
                            Logger.WriteLine($"Erro ao iniciar o bot :(", Color.IndianRed);
                            return;
                        }

                        if (BotEnvironment.Bot.Client.IsConnected)
                        {
                            string Message = Command.Substring(5);
                            if (Message.Length < 1)
                                Logger.WriteLine($"Você precisa colocar uma frase...", Color.IndianRed);
                            else
                            {
                                BotEnvironment.Bot.Client.SendMessage(BotEnvironment.Settings["twitch.channel.name"], Message);
                            }
                        }
                        else
                        {
                            Logger.WriteLine($"Bot não está conectado. Provavelmente o servidor da Twitch caiu.", Color.IndianRed);
                            Logger.WriteLine($"Tentando reconectar...", Color.IndianRed);
                            BotEnvironment.Bot.Client.Reconnect();
                        }
                        break;
                    case "help":
                        Logger.WriteLine("------------------------", Color.Cyan);
                        Logger.WriteLine("Comandos disponíveis:", Color.Cyan);
                        Logger.WriteLine("  clear - Limpa o console", Color.Cyan);
                        Logger.WriteLine("  force - Manda a mensagem de tempo em live e cigarros fumados", Color.Cyan);
                        Logger.WriteLine("  reconnect - Tenta reconectar o bot com o chat", Color.Cyan);
                        Logger.WriteLine("  logo - Mostra o logo", Color.Cyan);
                        Logger.WriteLine("  refresh - Atualiza os textos e configurações", Color.Cyan);
                        Logger.WriteLine("  talk <texto> - Permite enviar textos no chat. Não incluir '<' e '>'", Color.Cyan);
                        Logger.WriteLine("  uptime - Exibe o tempo que o bot está rodando", Color.Cyan);
                        Logger.WriteLine("------------------------", Color.Cyan);

                        break;

                    case "logo":
                        BotEnvironment.DrawLogo();
                        break;
                    default:
                        Logger.WriteLine($"Não existe o comando '{Parameters[0].ToLower()}'. Digite 'help' para exibir os comandos.", Color.IndianRed);
                        break;
                }
            }
            catch(Exception ex)
            {
                Logger.LogException($"Erro no comando {Command} : {ex.ToString()}");
            }            
        }

    }

}
