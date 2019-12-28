using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client.Events;

namespace BotdeFumar.Core.Commands
{
    public class MagicCommand : CommandBase
    {
        public override void Run(OnChatCommandReceivedArgs e)
        {
            //➰
            string[] possibleEmojis = BotEnvironment.Settings["bot.magic.emojis"].Split(',');
            string emoji = possibleEmojis[new Random().Next(possibleEmojis.Length - 1)];
            if (emoji == "!quote") {
                BotEnvironment.Bot.Client.SendMessage(BotEnvironment.Settings["twitch.channel.name"], $"peepoSmoke💨{emoji}");
            }
            else
            {
                BotEnvironment.Bot.Client.SendMessage(BotEnvironment.Settings["twitch.channel.name"], $"peepoSmoke 💨 {emoji}");
            }

        }
    }
}
