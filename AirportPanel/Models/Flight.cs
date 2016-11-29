using System;
using System.ComponentModel.DataAnnotations;

namespace AirportPanel.Models
{


    public enum FlightStatuses
    { CheckIn = 1, GateClosed, Arrived, Unknown, Canceled, Delayed, InFlight, Departed, Expected }


    public enum FlightDirections
    { Arrival = 1, Departure}

    public class Flight
    {
        public int Id { get; set; }
        public int FlightNumber { get; set; }
        public FlightDirections FlightDirection { get; set; }
        public FlightStatuses FlightStatus { get; set; }
        public DateTime DateAndTime { get; set; }
        public string CityPort { get; set; }
        public string Airline { get; set; }
        public string Terminal { get; set; }
        public string Gate { get; set; }

    }
}
