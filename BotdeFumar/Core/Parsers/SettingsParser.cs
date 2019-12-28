using BotdeFumar.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotdeFumar.Core.Parsers
{
    public class SettingsParser
    {
        public Dictionary<string, string> Settings = new Dictionary<string, string>();

        public void Load()
        {
            string path = "settings.ini";
            Settings.Clear();
            if(File.Exists(path))
            {
                foreach (string line in File.ReadAllLines(path))
                {
                    if (line.StartsWith("#") || !line.Contains("="))
                        continue;

                    string[] values = line.Split('=');
                    Settings.Add(values[0], values[1]);
                }
            }
            else
            {
                Logger.WriteLine($"Arquivo '{path}' não encontrado!", Color.OrangeRed);
            }
        }
    }
}
