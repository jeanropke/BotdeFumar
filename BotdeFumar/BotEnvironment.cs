using BotdeFumar.Core;
using BotdeFumar.Core.Parsers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Media;
using System.Threading;
using System.Windows.Media;

namespace BotdeFumar
{
    public static class BotEnvironment
    {
        private static SettingsParser settingsParser = new SettingsParser();
        private static TextsParser textsParser = new TextsParser();

        private static Bot bot;
        private static SoundPlayer player = new SoundPlayer();

        public static void Initialize()
        {
            CultureInfo ci = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;

            LoadSettings();
            LoadTexts();

            if (Settings["show.logo"] == "true")
                DrawLogo();

            if (!Directory.Exists("Logs"))
            {
                Directory.CreateDirectory("Logs");
            }

            bot = new Bot();


        }


        public static void DrawLogo()
        {
            string[] pepes = new string[] { "smoker", "monkaGun", "mariRage1" };
            Random random = new Random();
            Image img = Image.FromStream(new MemoryStream(Convert.FromBase64String(Resource.ResourceManager.GetObject(pepes[random.Next(pepes.Length)]).ToString())));
            DrawImage.ConsoleWriteImage(img);
            Console.ResetColor();
        }

        public static void LoadSettings()
        {
            settingsParser.Load();
        }

        public static void LoadTexts()
        {
            textsParser.Load();
        }

        public static string StartupTime()
        {
            return $"{GetStartupTime.Year}_{GetStartupTime.Month}_{GetStartupTime.Day}_{GetStartupTime.Hour}_{GetStartupTime.Minute}_{GetStartupTime.Second}";
        }

        public static void PlayAudio(List<string> list)
        {
            Random rand = new Random();
            BotEnvironment.PlayAudio(list[rand.Next(list.Count - 1)]);
        }

        public static void PlayAudio(string audio)
        {
            BotEnvironment.GetPlayer.SoundLocation = audio;
            BotEnvironment.GetPlayer.Play();
        }


        public static void EndProcessTree(string imageName)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "taskkill",
                Arguments = $"/im {imageName} /f /t",
                CreateNoWindow = true,
                UseShellExecute = false
            }).WaitForExit();
        }

        public static Dictionary<string, string> Settings { get; } = settingsParser.Settings;
        public static Dictionary<string, string> Texts { get; } = textsParser.Texts;
        public static DateTime GetStartupTime { get; } = DateTime.Now;
        public static Bot Bot { get { return bot; } }
        public static TimerDelay Timer { get { return new TimerDelay(); } }
        public static SoundPlayer GetPlayer { get { return player; } }
    }
}
