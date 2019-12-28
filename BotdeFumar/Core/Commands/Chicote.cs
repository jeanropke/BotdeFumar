using TwitchLib.Client.Events;

namespace BotdeFumar.Core.Commands
{
    public class Chicote : CommandBase
    {
        public override void Run(OnChatCommandReceivedArgs e)
        {

            if (BotEnvironment.Bot.CommandPermissionOverride[e.Command.CommandText])
            {
                BotEnvironment.PlayAudio("Audios/sfx/WHIP_CRACK_01.wav");
            }
            else
            {
                if (!(e.Command.ChatMessage.Username.ToLower() == "jean__" || e.Command.ChatMessage.IsBroadcaster || e.Command.ChatMessage.IsModerator))
                    return;

                BotEnvironment.PlayAudio("Audios/sfx/WHIP_CRACK_01.wav");
            }            
        }
    }
}
