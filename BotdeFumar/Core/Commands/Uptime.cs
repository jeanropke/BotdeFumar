using TwitchLib.Client.Events;

namespace BotdeFumar.Core.Commands
{
    public class Uptime : CommandBase
    {
        public override void Run(OnChatCommandReceivedArgs e)
        {
            if (!(e.Command.ChatMessage.Username.ToLower() == "jean__" || e.Command.ChatMessage.IsBroadcaster || e.Command.ChatMessage.IsModerator))
                return;
           
            BotEnvironment.Bot.GetLive_SendMessage(true);
            
        }
    }
}
