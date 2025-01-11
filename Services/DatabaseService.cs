using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;
using ColorMorph.Models;

namespace ColorMorph.Services
{
    public class DatabaseService
    {
        private readonly SQLiteAsyncConnection _database;

        public DatabaseService(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<ImageRecord>().Wait();
        }

        public Task<int> SaveImageRecordAsync(ImageRecord record) => _database.InsertAsync(record);

        public Task<List<ImageRecord>> GetImageRecordsAsync() => _database.Table<ImageRecord>().ToListAsync();

    }
}
