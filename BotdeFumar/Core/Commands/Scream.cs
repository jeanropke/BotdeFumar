using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client.Events;

namespace BotdeFumar.Core.Commands
{
    public class Scream : CommandBase
    {
        public override void Run(OnChatCommandReceivedArgs e)
        {
            if (BotEnvironment.Bot.CommandPermissionOverride[e.Command.CommandText])
            {
                this.Execute(e);
            }
            else
            {
                if (!(e.Command.ChatMessage.Username.ToLower() == "jean__" || e.Command.ChatMessage.IsBroadcaster || e.Command.ChatMessage.IsModerator))
                    return;
                this.Execute(e);                
            }
            
        }

        public void Execute(OnChatCommandReceivedArgs e)
        {
            long calc = ((DateTime.Now.Hour + 1) * (DateTime.Now.Minute + 1) * Hasher.Hash(e.Command.ChatMessage.Username)) / DateTime.Now.Day;
            if (e.Command.ArgumentsAsList.Count == 1)
            {
                if (long.TryParse(e.Command.ArgumentsAsList[0], out long arg))
                {
                    if (calc == arg)
                    {
                        if (BotEnvironment.Bot.ScreamAudio.Count > 0)
                        {
                            BotEnvironment.PlayAudio(BotEnvironment.Bot.ScreamAudio);
                        }
                    }
                }
            }
        }
    }
}
