using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotdeFumar.Core
{
    public class TimerDelay
    {

        public void UseDelay(int seconds)
        {
            if (seconds < 1) return;
            DateTime _desired = DateTime.Now.AddSeconds(seconds);
            while (DateTime.Now < _desired)
            {
            //    System.Windows.Forms.Application.DoEvents();
            }
        }

    }
}
