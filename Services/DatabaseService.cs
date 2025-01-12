using SQLite;

public class DatabaseService
{
    private readonly SQLiteConnection _db;

    public DatabaseService(string dbPath)
    {
        _db = new SQLiteConnection(dbPath);
        _db.CreateTable<ImageEntity>();
    }

    public int SaveImage(string name, byte[] data)
    {
        var image = new ImageEntity { Name = name, Data = data };
        return _db.Insert(image);
    }

    public ImageEntity GetImage(int id)
    {
        return _db.Find<ImageEntity>(id);
    }
}
