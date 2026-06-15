using SQLite;

namespace LocationHeatMapApp;

public class LocationDatabase
{
    SQLiteAsyncConnection database;

    public LocationDatabase()
    {
        string dbPath = Path.Combine(FileSystem.AppDataDirectory, "locations.db");

        database = new SQLiteAsyncConnection(dbPath);

        database.CreateTableAsync<LocationPoint>().Wait();
    }

    public async Task SaveLocation(LocationPoint point)
    {
        await database.InsertAsync(point);
    }

    public async Task<List<LocationPoint>> GetLocations()
    {
        return await database.Table<LocationPoint>().ToListAsync();
    }
}