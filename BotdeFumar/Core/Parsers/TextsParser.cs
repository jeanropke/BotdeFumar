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
    public class TextsParser
    {
        public Dictionary<string, string> Texts = new Dictionary<string, string>();

        public void Load()
        {
            string path = "texts.ini";
            Texts.Clear();
            if (File.Exists(path))
            {
                foreach (string line in File.ReadAllLines(path))
                {
                    if (line.StartsWith("#") || !line.Contains("="))
                        continue;

                    string[] values = line.Split('=');
                    Texts.Add(values[0], values[1]);
                }
            }
            else
            {
                Logger.WriteLine($"Arquivo '{path}' não encontrado!", Color.OrangeRed);
            }
        }
    }
}
