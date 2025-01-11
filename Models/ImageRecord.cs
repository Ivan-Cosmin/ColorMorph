using SQLite;

namespace ColorMorph.Models
{

    public class ImageRecord
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string FilePath { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
