using SQLite;
using System;

namespace LocationHeatMapApp.Models
{
    /// <summary>
    /// Represents the SQLite database schema for storing captured GPS entries.
    /// </summary>
    public class LocationPoint
    {
        // Automatically increments the ID primary key for every unique database row
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        // Stores the geometric coordinate values
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        // Tracks exactly when the position was captured for logging history
        public DateTime Timestamp { get; set; }
    }
}
