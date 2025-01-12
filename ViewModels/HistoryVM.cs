using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using ColorMorph.Services;
using Microsoft.Maui.Controls;

namespace ColorMorph.ViewModels
{
    public class HistoryVM : BaseVM
    {
        public ObservableCollection<ImageEntity> Images { get; private set; } = new ObservableCollection<ImageEntity>();

        public HistoryVM()
        {
            LoadImages();
        }

        public void LoadImages()
        {
            Images.Clear();
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "images.db");
            var dbService = new DatabaseService(dbPath);

            var images = dbService.GetAllImages();
            foreach (var image in images)
            {
                image.ImageSource = ImageSource.FromStream(() => new MemoryStream(image.Data));
                Images.Add(image);
            }
        }
    }
}