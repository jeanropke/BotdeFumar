using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client.Events;

namespace BotdeFumar.Core.Commands
{
    public class TestCommand : CommandBase
    {
        public override void Run(OnChatCommandReceivedArgs e)
        {
            Random rand = new Random();
            
            BotEnvironment.Bot.Client.SendMessage(BotEnvironment.Settings["twitch.channel.name"], $"1.{rand.Next(50, 99)}m");

        }
    }
}
