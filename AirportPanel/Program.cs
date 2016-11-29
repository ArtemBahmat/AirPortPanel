using AirportPanel.Models;
using ConsoleTables.Core;
using System;
using System.Collections.Generic;
using System.IO;

namespace AirportPanel
{
    class Program
    {
        static Type ThisType = typeof(Program);

        static void Main(string[] args)
        {
            int maxConsoleWidth = Console.LargestWindowWidth;  // на ноуте 170
            int maxConsoleHeight = Console.LargestWindowHeight;  // на ноуте 58

            if (maxConsoleWidth <= 170)
                Console.SetWindowSize(maxConsoleWidth, maxConsoleHeight);
            else
                Console.SetWindowSize(maxConsoleWidth - 20, Console.LargestWindowHeight-20);

            DBManager.SetAppDataDirectory();
            // DBManager.InitDB();

            BusinessLogic.ShowMainMenu();

            

            Console.ReadLine();
        }







    


    }
}
