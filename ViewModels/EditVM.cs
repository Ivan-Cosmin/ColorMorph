using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using ColorMorph.Services;
using Microsoft.Maui.Controls;

namespace ColorMorph.ViewModels
{
    public class Category
    {
        public required string Name { get; set; }
        public ICommand Command { get; set; } = null!;
    }

    public partial class EditVM : BaseVM
    {
        public ObservableCollection<Category> Categories { get; private set; } = new ObservableCollection<Category>();

        private readonly ImageEntity image;

        private ImageSource _imageSource = null!;
        public ImageSource ImageSource
        {
            get => _imageSource;
            set
            {
                _imageSource = value;
                OnPropertyChanged();
            }
        }

        public EditVM(Stream imageStream, ImageEntity image)
        {
            ImageSource = ImageSource.FromStream(() => imageStream);
            CreateCategories();
            this.image = image;
        }

        private void CreateCategories()
        {
            Categories = new ObservableCollection<Category>
            {
                new Category { Name = "ApplyGrayscale", Command = new Command(() => ApplyImageProcessing(ImageProcessingService.ApplyGrayscale)) },
                new Category { Name = "ApplyBlur", Command = new Command(() => ApplyImageProcessing(ImageProcessingService.ApplyBlur)) },
                new Category { Name = "DetectEdges", Command = new Command(() => ApplyImageProcessing(ImageProcessingService.DetectEdges)) },
                new Category { Name = "DetectFaces", Command = new Command(() => ApplyImageProcessing(ImageProcessingService.DetectFaces)) },
            };
        }

        private void ApplyImageProcessing(Func<byte[], byte[]> processingFunction)
        {
            // Assuming you have a way to get the byte array from the ImageSource
            var imageBytes = image.Data;
            var processedBytes = processingFunction(imageBytes);
            ImageSource = ImageSource.FromStream(() => new MemoryStream(processedBytes));
        }

    }
}