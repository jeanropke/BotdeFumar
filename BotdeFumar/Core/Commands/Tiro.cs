using System.Drawing;
using TwitchLib.Client.Events;

namespace BotdeFumar.Core.Commands
{
    public class Tiro : CommandBase
    {
        public override void Run(OnChatCommandReceivedArgs e)
        {

            if (BotEnvironment.Bot.CommandPermissionOverride[e.Command.CommandText])
            {
                BotEnvironment.PlayAudio(BotEnvironment.Bot.WeaponsAudios);              
            }
            else
            {
                if (!(e.Command.ChatMessage.Username.ToLower() == "jean__" || e.Command.ChatMessage.IsBroadcaster || e.Command.ChatMessage.IsModerator))
                    return;
              
                BotEnvironment.PlayAudio(BotEnvironment.Bot.WeaponsAudios);               
            }
        }
    }
}
