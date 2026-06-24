using SQLite;
using LocationHeatMapApp.Models;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace LocationHeatMapApp.Services
{
    /// <summary>
    /// Manages low-level asynchronous CRUD operations for the SQLite database engine.
    /// </summary>
    public class DatabaseService
    {
        private SQLiteAsyncConnection? _database;

        /// <summary>
        /// Asynchronously initializes the connection and verifies the existence of the table schema.
        /// </summary>
        private async Task InitAsync()
        {
            if (_database != null) return;

            // Establish an isolated storage path safe across multiple mobile platforms
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "UserLocations.db3");
            
            _database = new SQLiteAsyncConnection(dbPath);
            await _database.CreateTableAsync<LocationPoint>();
        }

        /// <summary>
        /// Inserts a new tracked coordinate position directly into the SQLite backend.
        /// </summary>
        public async Task<int> SaveLocationAsync(LocationPoint record)
        {
            await InitAsync();
            return await _database!.InsertAsync(record);
        }

        /// <summary>
        /// Retrieves every recorded history element ordered by the closest collection timestamps.
        /// </summary>
        public async Task<List<LocationPoint>> GetLocationsAsync()
        {
            await InitAsync();
            return await _database!.Table<LocationPoint>().ToListAsync();
        }
    }
}
