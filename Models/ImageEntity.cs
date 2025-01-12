using SQLite;

public class ImageEntity
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Name { get; set; }
    public byte[] Data { get; set; }

    [Ignore]
    public ImageSource ImageSource { get; set; }
}