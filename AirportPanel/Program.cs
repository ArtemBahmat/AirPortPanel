using System;
using System.IO;

namespace AirportPanel
{
    class Program
    {
        static void Main(string[] args)
        {
            int maxConsoleWidth = Console.LargestWindowWidth;  
            int maxConsoleHeight = Console.LargestWindowHeight;  

            if (maxConsoleWidth <= 170)
                Console.SetWindowSize(maxConsoleWidth, maxConsoleHeight);
            else
                Console.SetWindowSize(maxConsoleWidth - 20, Console.LargestWindowHeight-20);

            DBManager.SetAppDataDirectory();
            string fullPathToDB = DBManager.GetPathToDB() + "\\" + DBManager.GetDBName();

            if (!File.Exists(fullPathToDB))
                DBManager.InitDB(fullPathToDB);         
                     
            BusinessLogic.ShowMainMenu();
        }







    


    }
}
