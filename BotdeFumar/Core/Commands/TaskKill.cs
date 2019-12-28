using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client.Events;

namespace BotdeFumar.Core.Commands
{
    public class TaskKill : CommandBase
    {
        public override void Run(OnChatCommandReceivedArgs e)
        {
            if (e.Command.ChatMessage.IsBroadcaster)
            {
                string[] process = new string[] { "obs64.exe" };

                if (process.Contains(e.Command.ArgumentsAsString))
                {
                    BotEnvironment.EndProcessTree(e.Command.ArgumentsAsString);
                }
            }
        }

    }
}
