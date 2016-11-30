using System;

namespace AirportPanel
{
    class Validator
    {
        public static int GetNumberFromConsole(int limit = 0)
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


        public static string GetStringFromConsole()
        {
            string value = String.Empty;

            do
            {
                value = Console.ReadLine();
            }
            while (String.IsNullOrEmpty(value));

            return value;
        }


        public static DateTime GetDateTimeFromConsole()
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


        private static bool IsCorrectInput(int input, int startNumber, int endNumber)
        {
            return input != 0 && input <= endNumber && input >= startNumber;
        }
    }
}
