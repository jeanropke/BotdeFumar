using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client.Events;

namespace BotdeFumar.Core.Commands
{
    public abstract class CommandBase
    {
        public abstract void Run(OnChatCommandReceivedArgs e);
    }
}
