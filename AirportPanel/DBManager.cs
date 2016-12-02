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
        static readonly string msgSavedSuccess = "Data was saved to DB successfully.";
        static readonly string msgDBCreatedSuccess = "DB was created successfully.";
        static readonly string msgDeletedSuccess = "Data was deleted successfully.";
        static readonly string msgError = "Error proceeding.";


        static DBManager()
        {
            string pathToDB = GetPathToDB();
            AppDomain.CurrentDomain.SetData("DataDirectory", pathToDB);
            string fullPathToDB = pathToDB + "\\" + GetDBName();

            if (!File.Exists(fullPathToDB))
            {
                InitDB(fullPathToDB);
            }
        }


        public static void SaveFlightToDB(Flight flight)
        {
            if (flight != null)
            {
                try
                {
                    using (var db = new AirportContext())
                    {
                        db.Flights.Add(flight);
                        db.SaveChanges();
                    }

                    Log.Info(msgSavedSuccess);
                }
                catch (Exception ex)
                {                     
                    Log.Error(msgError, ex);
                }
            }
        }


        public static void SaveFlightesToDB(List<Flight> flightes)
        {
            try
            {
                using (var db = new AirportContext())
                {
                    db.Flights.AddRange(flightes);
                    db.SaveChanges();
                }

                Log.Info(msgSavedSuccess);           
            }
            catch (Exception ex)
            {
                Log.Error(msgError, ex);
            }
        }


        public static void UpdateFlightToDB(Flight flight)
        {
            if (flight != null)
            {
                try
                {
                    using (var db = new AirportContext())
                    {
                        db.Flights.Attach(flight);
                        db.Entry(flight).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    Log.Info(msgSavedSuccess);
                }
                catch (Exception ex)
                {
                    Log.Error(msgError, ex);
                }
            }
        }


        public static void DeleteFlightFromDB(Flight flight)
        {
            if (flight != null)
            {
                try
                {
                    using (var db = new AirportContext())
                    {
                        var entry = db.Entry(flight);

                        if (entry.State == EntityState.Detached)
                            db.Flights.Attach(flight);

                        db.Flights.Remove(flight);
                        db.SaveChanges();
                    }

                    Log.Info(msgDeletedSuccess);
                }
                catch (Exception ex)
                {
                    Log.Error(msgError, ex);
                }
            }
        }


        public static List<Flight> GetFlightsFromDB(FlightDirections arrival)
        {
            List<Flight> flightes = new List<Flight>();

            try
            {
                using (var db = new AirportContext())
                {
                    db.Configuration.AutoDetectChangesEnabled = false;

                    flightes = db.Flights.Where(f => f.FlightDirection == arrival)
                                         .OrderBy(f => f.DateAndTime)
                                         .ToList();
                }
            }
            catch (Exception ex)
            {
                Log.Error(msgError, ex);
            }

            return flightes;
        }


        public static List<Flight> GetFlightsFromDB(DateTime dateTime, bool isNearest = false, int extraHours = 1)
        {
            List<Flight> flights = new List<Flight>();
            DateTime tillDateTime;

            try
            {
                using (var db = new AirportContext())
                {
                    db.Configuration.AutoDetectChangesEnabled = false;

                    if (!isNearest)
                    {
                        flights = db.Flights.Where(f => f.DateAndTime == dateTime)
                                            .OrderBy(f => f.DateAndTime)
                                            .ToList();
                    }
                    else
                    {
                        tillDateTime = dateTime.AddHours(extraHours);
                        flights = db.Flights.Where(f => f.DateAndTime <= tillDateTime && f.DateAndTime >= dateTime)
                                            .OrderBy(f => f.DateAndTime)
                                            .ToList();
                    }
                    
                }
            }
            catch (Exception ex)
            {
                Log.Error(msgError, ex);
            }

            return flights;
        }


        public static List<Flight> GetFlightsFromDB(string cityPort)
        {
            List<Flight> flightes = new List<Flight>();

            if (!String.IsNullOrEmpty(cityPort))
            {
                try
                {
                    using (var db = new AirportContext())
                    {
                        db.Configuration.AutoDetectChangesEnabled = false;

                        flightes = db.Flights.Where(f => f.CityPort == cityPort)
                                             .OrderBy(f => f.DateAndTime)
                                             .ToList();
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(msgError, ex);
                }
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

                    flightes = db.Flights.ToList();
                }
            }
            catch (Exception ex)
            {
                Log.Error(msgError, ex);
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

                    flight = db.Flights.Where(f => f.FlightId == flightNumber).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Log.Error(msgError, ex);
            }

            return flight;
        }


        private static void InitDB(string pathToDB)
        {
            List<Flight> flightes = new List<Flight>();

            using (AirportContext db = new AirportContext())
            {
                flightes.Add(new Flight
                {
                    RunwayNumber = 1,
                    FlightDirection = FlightDirections.Arrival,
                    FlightStatus = FlightStatuses.Expected,
                    DateAndTime = DateTime.Parse("30.11.2016 13:00"),
                    CityPort = "Kharkov",
                    Airline = "UkrainAirlines",
                    Terminal = "Some Terminal № 1",
                    Gate = "Some Gate № 1"
                });

                flightes.Add(new Flight
                {
                    RunwayNumber = 2,
                    FlightDirection = FlightDirections.Arrival,
                    FlightStatus = FlightStatuses.Delayed,
                    DateAndTime = DateTime.Parse("01.12.2016 10:30"),
                    CityPort = "Kharkov",
                    Airline = "AmericanAirlines",
                    Terminal = "Some Terminal № 2",
                    Gate = "Some Gate № 2"
                });

                flightes.Add(new Flight
                {
                    RunwayNumber = 4,
                    FlightDirection = FlightDirections.Departure,
                    FlightStatus = FlightStatuses.InFlight,
                    DateAndTime = DateTime.Parse("01.12.2016 11:00"),
                    CityPort = "Odessa",
                    Airline = "RussianAirlines",
                    Terminal = "Some Terminal № 11",
                    Gate = "Some Gate № 11"
                });

                flightes.Add(new Flight
                {
                    RunwayNumber = 3,
                    FlightDirection = FlightDirections.Departure,
                    FlightStatus = FlightStatuses.Canceled,
                    DateAndTime = DateTime.Parse("01.12.2016 10:45"),
                    CityPort = "Kiev",
                    Airline = "TurkishAirlines",
                    Terminal = "Some Terminal № 10",
                    Gate = "Some Gate № 10"
                });

                flightes.Add(new Flight
                {
                    RunwayNumber = 4,
                    FlightDirection = FlightDirections.Departure,
                    FlightStatus = FlightStatuses.InFlight,
                    DateAndTime = DateTime.Parse("01.12.2016 23:30"),
                    CityPort = "Odessa",
                    Airline = "RussianAirlines",
                    Terminal = "Some Terminal № 12",
                    Gate = "Some Gate № 12"
                });

                flightes.Add(new Flight
                {
                    RunwayNumber = 3,
                    FlightDirection = FlightDirections.Departure,
                    FlightStatus = FlightStatuses.Expected,
                    DateAndTime = DateTime.Parse("02.12.2016 18:10"),
                    CityPort = "Poltava",
                    Airline = "RussianAirlines",
                    Terminal = "Some Terminal № 13",
                    Gate = "Some Gate № 13"
                });

                SaveFlightesToDB(flightes);
            }

            if (File.Exists(pathToDB))
            {
                Log.Info(msgDBCreatedSuccess);
            }
            else
            {
                Log.Error(msgError);
            }
        }


        private static string GetConnectionString()
        {
            string connectionString = string.Empty;

            using (var db = new AirportContext())
            {
                connectionString = db.Database.Connection.ConnectionString;
            }

            return connectionString;
        }


        private static string GetPathToDB()
        {
            string relative = @"..\..\App_Data";
            string absolute = Path.GetFullPath(relative);
            return absolute;
        }


        private static string GetDBName()
        {
            string connectionString = GetConnectionString();
            int startIndex = connectionString.LastIndexOf("\\");
            int endIndex = connectionString.LastIndexOf("'") - 1;
            string fileName = connectionString.Substring(startIndex + 1, endIndex - startIndex);
            return fileName;
        }
    }
}
