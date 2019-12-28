using System.Reflection;
using TwitchLib.Client.Events;

namespace BotdeFumar.Core.Commands
{
    public class BotVersion : CommandBase
    {
        public override void Run(OnChatCommandReceivedArgs e)
        {
            if (!(e.Command.ChatMessage.Username.ToLower() == "jean__"))
                return;

            BotEnvironment.Bot.Client.SendMessage(BotEnvironment.Settings["twitch.channel.name"], Assembly.GetExecutingAssembly().GetName().Version.ToString());

        }
    }
}
