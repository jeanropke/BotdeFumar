using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client.Events;

namespace BotdeFumar.Core.Commands
{
    public class Permission : CommandBase
    {
        public override void Run(OnChatCommandReceivedArgs e)
        {
            if (!(e.Command.ChatMessage.Username.ToLower() == "jean__" || e.Command.ChatMessage.IsBroadcaster || e.Command.ChatMessage.IsModerator))
                return;

            List<string> args = e.Command.ArgumentsAsList;

            Console.WriteLine(args.Count);

            if (args.Count == 2)
            {
                if (BotEnvironment.Bot.Commands.ContainsKey(args[0]))
                {
                    if (BotEnvironment.Bot.CommandPermissionOverride.ContainsKey(args[0]))
                    {
                        //Console.WriteLine($"Comando {args[0]} encontrado. Removendo permissão...");
                        BotEnvironment.Bot.CommandPermissionOverride.Remove(args[0]);
                    }
                    //Console.WriteLine($"Adicionando override de permissão...");
                    BotEnvironment.Bot.CommandPermissionOverride.Add(args[0], args[1] == "true");
                }
                else
                {
                    //Console.WriteLine($"Comando {args[0]} não encontrado FeelsMan");
                }
            }
            
        }
    }
}
