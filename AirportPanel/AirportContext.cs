using AirportPanel.Models;
using System.Data.Entity;

namespace AirportPanel
{
    class AirportContext : DbContext
    {
        public AirportContext()
            : base("DBConnectionAirport")
        { }

        public DbSet<Flight> Flights { get; set; }
    }
}