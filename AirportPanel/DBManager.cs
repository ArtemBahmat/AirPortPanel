using AirportPanel.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;

namespace AirportPanel
{
    static class DBManager
    {
        static Type ThisType = typeof(DBManager);
        static string msgAnyKeyGoHome = "Press Any key to go to main menu.";
        static string msgSavedSuccessfully = "Data was saved to DB successfully. ";
        static string msgDBCreatedSuccessfully = "DB was created successfully. ";
        static string msgDeletedSuccessfully = "Data was deleted successfully. ";
        static string msgError = "Error proceeding. ";


        public static void SaveFlightToDB(Flight flight)
        {
            try
            {
                using (var db = new AirportContext())
                {
                    db.Flightes.Add(flight);
                    db.SaveChanges();
                }

                Console.WriteLine(msgSavedSuccessfully + msgAnyKeyGoHome);
                Log.For(ThisType).Info(msgSavedSuccessfully);
            }
            catch (Exception ex)
            {
                Console.WriteLine(msgError + msgAnyKeyGoHome);
                Log.For(ThisType).Error(ex);
            }
        }


        public static void SaveFlightesToDB(List<Flight> flightes)
        {
            try
            {
                using (var db = new AirportContext())
                {
                    db.Flightes.AddRange(flightes);
                    db.SaveChanges();
                }

                Console.WriteLine(msgSavedSuccessfully + msgAnyKeyGoHome);
                Log.For(ThisType).Info(msgSavedSuccessfully);
            }
            catch (Exception ex)
            {
                Console.WriteLine(msgError + msgAnyKeyGoHome);
                Log.For(ThisType).Error(ex);
            }

        }


        public static void DeleteFlightFromDB(Flight flight)
        {
            try
            {
                using (var db = new AirportContext())
                {
                    var entry = db.Entry(flight);

                    if (entry.State == EntityState.Detached)
                        db.Flightes.Attach(flight);

                    db.Flightes.Remove(flight);
                    db.SaveChanges();
                }

                Console.WriteLine(msgDeletedSuccessfully + msgAnyKeyGoHome);
                Log.For(ThisType).Info(msgDeletedSuccessfully);
            }
            catch (Exception ex)
            {
                Console.WriteLine(msgError + msgAnyKeyGoHome);
                Log.For(ThisType).Error(ex);
            }
        }



        internal static List<Flight> GetFlightsFromDB(FlightDirections arrival)
        {
            List<Flight> flightes = new List<Flight>();

            try
            {
                using (var db = new AirportContext())
                {
                    db.Configuration.AutoDetectChangesEnabled = false;

                    flightes = db.Flightes.Select(f => f).Where(f => f.FlightDirection == arrival).ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(msgError + msgAnyKeyGoHome);
                Log.For(ThisType).Error(ex);
            }

            return flightes;
        }



        public static List<Flight> GetAllFlightsFromDB()
        {
            List<Flight> flightes = new List<Flight>();

            try
            {
                using (var db = new AirportContext())
                {
                    db.Configuration.AutoDetectChangesEnabled = false;

                    flightes = db.Flightes.Select(f => f).ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(msgError + msgAnyKeyGoHome);
                Log.For(ThisType).Error(ex);
            }

            return flightes;
        }


        public static Flight GetFlightFromDB(int flightNumber)
        {
            Flight flight = new Flight();

            try
            {
                using (var db = new AirportContext())
                {
                    db.Configuration.AutoDetectChangesEnabled = false;

                    flight = db.Flightes.Select(f => f).Where(f => f.FlightNumber == flightNumber).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(msgError + msgAnyKeyGoHome);
                Log.For(ThisType).Error(ex);
            }

            return flight;
        }




        public static void SetAppDataDirectory()
        {
            string relative = @"..\..\App_Data";
            string absolute = Path.GetFullPath(relative);
            AppDomain.CurrentDomain.SetData("DataDirectory", absolute);
        }


        public static void InitDB()
        {
            List<Flight> flightes = new List<Flight>();

            using (AirportContext db = new AirportContext())
            {
                flightes.Add(new Flight
                {
                    FlightDirection = FlightDirections.Arrival,
                    FlightNumber = 300,
                    FlightStatus = FlightStatuses.Expected,
                    DateAndTime = DateTime.Now,
                    CityPort = "Kharkov",
                    Airline = "UkrainAirlines",
                    Terminal = "Some Terminal № 1",
                    Gate = "Some Gate № 1"
                });

                flightes.Add(new Flight
                {
                    FlightDirection = FlightDirections.Arrival,
                    FlightNumber = 301,
                    FlightStatus = FlightStatuses.Delayed,
                    DateAndTime = DateTime.Now,
                    CityPort = "Kharkov",
                    Airline = "AmericanAirlines",
                    Terminal = "Some Terminal № 2",
                    Gate = "Some Gate № 2"
                });

                flightes.Add(new Flight
                {
                    FlightDirection = FlightDirections.Departure,
                    FlightNumber = 401,
                    FlightStatus = FlightStatuses.Canceled,
                    DateAndTime = DateTime.Now,
                    CityPort = "Kiev",
                    Airline = "TurkishAirlines",
                    Terminal = "Some Terminal № 10",
                    Gate = "Some Gate № 10"
                });

                flightes.Add(new Flight
                {
                    FlightDirection = FlightDirections.Departure,
                    FlightNumber = 402,
                    FlightStatus = FlightStatuses.InFlight,
                    DateAndTime = DateTime.Now,
                    CityPort = "Odessa",
                    Airline = "RussianAirlines",
                    Terminal = "Some Terminal № 11",
                    Gate = "Some Gate № 11"
                });

                DBManager.SaveFlightesToDB(flightes);
            }

            object obj = AppDomain.CurrentDomain.GetData("DataDirectory");
            string dbPath = obj.ToString() + "\\" + DBManager.GetDBName();

            if (File.Exists(dbPath))
            {
                Log.For(ThisType).Info(msgDBCreatedSuccessfully);
            }
            else
                Log.For(ThisType).Error(msgError);
        }


        public static string GetConnectionString()
        {
            string connectionString = string.Empty;

            using (var db = new AirportContext())
            {
                connectionString = db.Database.Connection.ConnectionString;
            }

            return connectionString;
        }



        public static string GetDBName()
        {
            string connectionString = GetConnectionString();
            int startIndex = connectionString.LastIndexOf("\\");
            int endIndex = connectionString.LastIndexOf("'") - 1;
            string fileName = connectionString.Substring(startIndex + 1, endIndex - startIndex);
            return fileName;
        }



    }
}
