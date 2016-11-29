using AirportPanel.Models;
using ConsoleTables.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportPanel
{
    static class BusinessLogic
    {
        public static void ShowMainMenu()
        {
            int choice = 0;

            Console.WriteLine("============ What do you want to do: ============");
            Console.WriteLine("1. View all the flights");
            Console.WriteLine("2. View the flight information about arrivals");
            Console.WriteLine("3. View the flight information about departures");
            Console.WriteLine("4. Adding information");
            Console.WriteLine("5. Deleting information");
            Console.WriteLine("6. Editing information");
            Console.WriteLine("7. Quit");

            choice = GetIntegerNumberFromUser(6);

            switch (choice)
            {
                case 1:
                    ShowFlightes(DBManager.GetAllFlightsFromDB());
                    break;
                case 2:
                    ShowFlightesByDirection(FlightDirections.Arrival);
                    break;
                case 3:
                    ShowFlightesByDirection(FlightDirections.Departure);
                    break;
                case 4:
                    AddData();
                    break;
                case 5:
                    DeleteData();
                    break;
                case 6:
                    EditData();
                    break;
                case 7:
                    Environment.Exit(0);
                    break;
            }

            ShowMainMenu();
        }

        private static void EditData()
        {
            Console.Clear();
            Console.WriteLine("============ What do you want to edit: ============");
            Console.WriteLine("Enter flight number of the flight you want to edit:");

            int flightNumber = GetIntegerNumberFromUser();
            Flight flight = DBManager.GetFlightFromDB(flightNumber);

            if (flight != null)
            {
                Console.WriteLine("Flight direction:");
                Console.Write(flight.FlightDirection);
                flight.FlightDirection = GetIntegerNumberFromUser();
                
            }

        }

        private static void AddData()
        {
            Console.Clear();
            Console.WriteLine("============ What do you want to add: ============");
            Console.WriteLine("1. Adding data to arrivals");
            Console.WriteLine("2. Adding data to departures");
            Console.WriteLine("3. Return to main menu");

            switch (GetIntegerNumberFromUser(3))
            {
                case 1:
                    Console.Clear();
                    Console.WriteLine("Adding data to arrivals");
                    GetFlightFromUser(FlightDirections.Arrival);
                    break;
                case 2:
                    Console.Clear();
                    Console.WriteLine("Adding data to departures");
                    GetFlightFromUser(FlightDirections.Departure);
                    break;
                case 3:
                    GoHome();
                    break;
            }
        }


        private static void DeleteData()
        {
            Console.Clear();
            Console.WriteLine("============ What do you want to delete: ============");
            Console.WriteLine("Enter flight number of the flight you want to delete:");

            int flightNumber = GetIntegerNumberFromUser();
            Flight flight = DBManager.GetFlightFromDB(flightNumber);

            if (flight != null)
            {
                Console.WriteLine("You want to delete this row:");
                ShowFlight(flight);
                Console.WriteLine("Are you sure for deleting? 1 - YES, 2 - NO");
                int choice = GetIntegerNumberFromUser(2);

                if (choice == 1)
                    DBManager.DeleteFlightFromDB(flight);
                else
                    Console.WriteLine("Deleting cancelled. Press any key to go to main menu.");
            }
            else
                Console.WriteLine("Flight with such number was not found. Press any key to go to main menu.");


            Console.ReadLine();
            GoHome();

        }







        private static void GetFlightFromUser(FlightDirections direction)
        {
            Flight flight = new Flight();

            flight.FlightDirection = direction;

            Console.WriteLine("Enter flight number:");
            flight.FlightNumber = GetIntegerNumberFromUser();

            Console.WriteLine("Enter name of city port:");
            flight.CityPort = Console.ReadLine();

            Console.WriteLine("Enter name of airline:");
            flight.Airline = Console.ReadLine();

            Console.WriteLine("Enter name of terminal:");
            flight.Terminal = Console.ReadLine();

            Console.WriteLine("Enter name of gate:");
            flight.Gate = Console.ReadLine();

            Console.WriteLine("Enter date and time in such format: 01.01.2016 13:30");
            flight.DateAndTime = GetDateFromUser();

            Console.WriteLine("Enter flight status number (1 = CheckIn, 2 = GateClosed, 3 = Arrived, 4 = Unknown, 5 = Canceled, 6 = Delayed, 7 = InFlight, 8 = Departed, 9 = Expected)");
            flight.FlightStatus = (FlightStatuses)GetIntegerNumberFromUser(9);

            DBManager.SaveFlightToDB(flight);

            Console.ReadLine();
            GoHome();
        }



        private static int GetIntegerNumberFromUser(int limit = 0)
        {
            int value = 0;
            int startIndex = 1;
            bool isDigit = false;
            bool validInput = false;

            do
            {
                isDigit = int.TryParse(Console.ReadLine(), out value);
                validInput = (isDigit && limit != 0) ? IsCorrectInput(value, startIndex, limit) : isDigit;

            }
            while (!validInput);

            return value;
        }


        private static bool IsCorrectInput(int input, int startNumber, int endNumber)
        {
            return input != 0 && input <= endNumber && input >= startNumber;
        }


        private static DateTime GetDateFromUser()
        {
            DateTime date;
            bool isDate = false;

            do
            {
                isDate = DateTime.TryParse(Console.ReadLine(), out date);
            }
            while (!isDate);

            return date;
        }



        private static void GoHome()
        {
            Console.Clear();
            ShowMainMenu();
        }

        private static void ShowFlightesByDirection(FlightDirections direction)
        {
            List<Flight> flightes = DBManager.GetFlightsFromDB(direction);
            ConsoleTable.From<Flight>(flightes).Write();
        }

        private static void ShowFlightes(List<Flight> flightes)
        {
            ConsoleTable.From<Flight>(flightes).Write();
        }


        private static void ShowFlight(Flight flight)
        {
            List<Flight> flightes = new List<Flight>();

            if (flight != null)
            {
                flightes.Add(flight);
                ShowFlightes(flightes);
            }
            else
                Console.WriteLine("Such flight was not found");
        }






    }
}
