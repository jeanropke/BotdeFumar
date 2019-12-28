using BotdeFumar.Core;
using System;
using System.Diagnostics;

namespace BotdeFumar
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Bot de Fumar";

            try
            {
                BotEnvironment.Initialize();
            }
            catch (Exception ex)
            {
                Logger.LogException($"Não foi possível iniciar o bot: {ex.ToString()}");
            }
            while (true)
            {
                ConsoleCommand.InvokeCommand(Console.ReadLine());
            }
        }
    }
}
