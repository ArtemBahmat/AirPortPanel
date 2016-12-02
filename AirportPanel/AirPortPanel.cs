using AirportPanel.Models;
using ConsoleTables.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AirportPanel
{
    static class AirPortPanel
    {
        public static void ShowMainMenu()
        {
            int input = 0;
            bool toExit = false;

            while (!toExit)
            {
                Console.Clear();
                Console.WriteLine("============ What do you want to do: ============");
                Console.WriteLine("1. View all the flights");
                Console.WriteLine("2. View the flight information about arrivals");
                Console.WriteLine("3. View the flight information about departures");
                Console.WriteLine("4. Adding the flight");
                Console.WriteLine("5. Deleting the flight");
                Console.WriteLine("6. Editing  the flight");
                Console.WriteLine("7. Searching for the flight");
                Console.WriteLine("8. Show emergency message");
                Console.WriteLine("9. Quit");

                input = Validator.GetNumberFromConsole(9);
                Console.Clear();

                switch (input)
                {
                    case 1:
                        ShowFlights(DBManager.GetAllFlightsFromDB());
                        break;
                    case 2:
                        ShowFlightsByDirection(FlightDirections.Arrival);
                        break;
                    case 3:
                        ShowFlightsByDirection(FlightDirections.Departure);
                        break;
                    case 4:
                        AddFlight();
                        break;
                    case 5:
                        DeleteFlight();
                        break;
                    case 6:
                        EditFlight();
                        break;
                    case 7:
                        FindFlight();
                        break;
                    case 8:
                        ShowEmergencyInfo();
                        break;
                    case 9:
                        toExit = true;
                        break;
                }
                PauseConsole();
            }
        }


        private static void PauseConsole()
        {
            Console.WriteLine("Press any key to continue");
            Console.ReadLine();
        }


        private static void ShowEmergencyInfo()
        {
            string msgEmergency = "============  EMERGENCY !!! PLEASE EVACUATE OFF THE BUILDNG !!! ============";

            Console.Beep();
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition((Console.WindowWidth - msgEmergency.Length) / 2, Console.LargestWindowHeight / 2);
            Console.WriteLine(msgEmergency);
            Console.ReadLine();
            Console.ResetColor();
        }


        private static void FindFlight()
        {
            int input = 0;
            List<Flight> flights = new List<Flight>();

            Console.WriteLine("============ Searching flight(s) ============");
            Console.WriteLine("Select serching criterian:");
            Console.WriteLine("1. Nearest flights");                    // multiple                   
            Console.WriteLine("2. By flight number");                   // single
            Console.WriteLine("3. By date and time of arrival");        // multiple
            Console.WriteLine("4. By City Port");                       // multiple
            Console.WriteLine("5. Return to main menu");
            input = Validator.GetNumberFromConsole(5);

            switch (input)
            {
                case 1:
                    flights = GetFlightsByDateTime(true);
                    break;
                case 2:
                    Flight flight = GetFlightByNumber();
                    if (flight != null)
                        flights.Add(flight);
                    break;
                case 3:
                    flights = GetFlightsByDateTime();
                    break;
                case 4:
                    flights = GetFlightsByCityPort();
                    break;
                case 5:
                    return;
            }

            if (flights.Count > 0)
            {
                ShowFlights(flights);
            }
            else
            {
                Console.WriteLine("Flight(es) was/were not found");
            }
        }


        private static List<Flight> GetFlightsByCityPort()
        {
            Console.WriteLine("Enter City Port of the flight:");
            string cityPort = Validator.GetStringFromConsole();
            return DBManager.GetFlightsFromDB(cityPort);
        }


        private static List<Flight> GetFlightsByDateTime(bool isNearest = false)
        {
            List<Flight> result = new List<Flight>();
            int extraHours;

            Console.WriteLine("Enter date and time in format dd.MM.YYYY hh:mm");
            DateTime date = Validator.GetDateTimeFromConsole();

            if (isNearest)
            {
                Console.WriteLine("Enter number of extra hours from 1 till 12:");
                extraHours = Validator.GetNumberFromConsole(12);
                result = DBManager.GetFlightsFromDB(date, isNearest, extraHours);
            }
            else
            {
                result = DBManager.GetFlightsFromDB(date);
            }

            return result;
        }


        private static Flight GetFlightByNumber()
        {
            Console.WriteLine("Enter flight number:");
            int number = Validator.GetNumberFromConsole();
            return DBManager.GetFlightFromDB(number);
        }


        private static void EditFlight()
        {
            Console.Clear();
            Console.WriteLine("============ Editing flight ============");
            Console.WriteLine("Enter flight number of the flight you want to edit:");

            int flightNumber = Validator.GetNumberFromConsole();
            Flight flight = DBManager.GetFlightFromDB(flightNumber);

            if (flight != null)
            {
                Console.WriteLine("Number of runway:");
                Console.WriteLine($"Current value: {flight.RunwayNumber}");
                Console.WriteLine("New value:");
                flight.RunwayNumber = Validator.GetNumberFromConsole();

                Console.WriteLine("Flight status number (1 - CheckIn, 2 - GateClosed, 3 - Arrived, 4 - Unknown, 5 - Canceled, 6 - Delayed, 7 - InFlight, 8 - Departed, 9 - Expected)");
                Console.WriteLine($"Current value: {flight.FlightStatus}");
                Console.WriteLine("New value:");
                flight.FlightStatus = (FlightStatuses)Validator.GetNumberFromConsole(9);

                Console.WriteLine("Flight date and time:");
                Console.WriteLine($"Current value: {flight.DateAndTime}");
                Console.WriteLine("New value:");
                flight.DateAndTime = Validator.GetDateTimeFromConsole();

                Console.WriteLine("City port:");
                Console.WriteLine($"Current value: {flight.CityPort}");
                Console.WriteLine("New value:");
                flight.CityPort = Validator.GetStringFromConsole();

                Console.WriteLine("Airline:");
                Console.WriteLine($"Current value: {flight.Airline}");
                Console.WriteLine("New value:");
                flight.Airline = Validator.GetStringFromConsole();

                Console.WriteLine("Terminal:");
                Console.WriteLine($"Current value: {flight.Terminal}");
                Console.WriteLine("New value:");
                flight.Terminal = Validator.GetStringFromConsole();

                Console.WriteLine("Gate:");
                Console.WriteLine($"Current value: {flight.Gate}");
                Console.WriteLine("New value:");
                flight.Gate = Validator.GetStringFromConsole();

                DBManager.UpdateFlightToDB(flight);
            }
            else
            {
                Console.WriteLine("The flight with such number wasn't found");
            }
        }


        private static void AddFlight()
        {
            Console.Clear();
            Console.WriteLine("============ What do you want to add: ============");
            Console.WriteLine("1. Adding data to arrivals");
            Console.WriteLine("2. Adding data to departures");
            Console.WriteLine("3. Return to main menu");
            int input = Validator.GetNumberFromConsole(3);
            Console.Clear();

            switch (input)
            {
                case 1:
                    Console.WriteLine("=========== Adding data to arrivals ===========");
                    GetFlightDataFromUser(FlightDirections.Arrival);
                    break;
                case 2:
                    Console.WriteLine("=========== Adding data to departures ===========");
                    GetFlightDataFromUser(FlightDirections.Departure);
                    break;
                case 3:
                    break;
            }
        }


        private static void DeleteFlight()
        {
            Console.Clear();
            Console.WriteLine("============ What do you want to delete: ============");
            Console.WriteLine("Enter flight number of the flight you want to delete:");

            int flightNumber = Validator.GetNumberFromConsole();
            Flight flight = DBManager.GetFlightFromDB(flightNumber);

            if (flight != null)
            {
                Console.WriteLine("You want to delete this row:");
                ShowFlights(flight);
                Console.WriteLine("Are you sure for deleting? 1 - YES, 2 - NO");
                int input = Validator.GetNumberFromConsole(2);

                if (input == 1)
                {
                    DBManager.DeleteFlightFromDB(flight);
                }
                else
                {
                    Console.WriteLine("Deleting cancelled");
                }
            }
            else
            {
                Console.WriteLine("Flight with such number was not found");
            }
        }


        private static void GetFlightDataFromUser(FlightDirections direction)
        {
            Flight flight = new Flight() {  FlightDirection = direction};            

            Console.WriteLine("Enter number of runway:");
            flight.RunwayNumber = Validator.GetNumberFromConsole();

            Console.WriteLine("Enter name of city port:");
            flight.CityPort = Validator.GetStringFromConsole();

            Console.WriteLine("Enter name of airline:");
            flight.Airline = Validator.GetStringFromConsole();

            Console.WriteLine("Enter name of terminal:");
            flight.Terminal = Validator.GetStringFromConsole();

            Console.WriteLine("Enter name of gate:");
            flight.Gate = Validator.GetStringFromConsole();

            Console.WriteLine("Enter date and time in such format: dd.MM.YYYY hh:mm");
            flight.DateAndTime = Validator.GetDateTimeFromConsole();

            Console.WriteLine("Enter flight status number (1 - CheckIn, 2 - GateClosed, 3 - Arrived, 4 - Unknown, 5 - Canceled, 6 - Delayed, 7 - InFlight, 8 - Departed, 9 - Expected)");
            flight.FlightStatus = (FlightStatuses)Validator.GetNumberFromConsole(9);

            DBManager.SaveFlightToDB(flight);
        }


        private static void ShowFlightsByDirection(FlightDirections direction)
        {
            Console.Clear();
            List<Flight> flights = DBManager.GetFlightsFromDB(direction);
            ShowFlights(flights);
        }


        private static void ShowFlights(List<Flight> flights)
        {
            if (flights != null && flights.Count > 0)
            {
                ConsoleTable.From<Flight>(flights).Write();
            }                      
        }


        private static void ShowFlights(params Flight[] flights)
        {          
            ShowFlights(flights.OfType<Flight>().ToList());
        }    
    }
}
