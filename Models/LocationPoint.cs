using SQLite;

namespace LocationHeatMapApp;

public class LocationPoint
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public string TimeRecorded { get; set; }
}