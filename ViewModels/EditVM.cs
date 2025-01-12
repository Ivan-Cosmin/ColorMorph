using System.Collections.ObjectModel;
using System.Windows.Input;
using ColorMorph.Services;
using Plugin.LocalNotification;

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

        private readonly ImageEntity _originalImage;
        private ImageEntity _processedImage;

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
        public ICommand ShowOriginalImageCommand { get; }
        public ICommand ShowProccesedImageCommand { get; }
        public ICommand ResetImageCommand { get; }
        public ICommand SaveImageCommand {  get; }

        public EditVM(Stream imageStream, ImageEntity image)
        {
            ImageSource = ImageSource.FromStream(() => imageStream);
            CreateCategories();
            this._originalImage = image;
            this._processedImage = new ImageEntity { Data = image.Data };

            ShowOriginalImageCommand = new Command(ShowOriginal);
            ShowProccesedImageCommand = new Command(ShowProccesed);
            ResetImageCommand = new Command(ResetImage);
            SaveImageCommand = new Command(SaveImage);
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

        private void SaveImageOnDB()
        {
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "images.db");
            var dbService = new DatabaseService(dbPath);

            try
            {
                int id = dbService.SaveImage(_processedImage.Name, _processedImage.Data);

                if (id >= 0)
                {
                    Console.WriteLine("Image saved to database successfully.");
                }
                else
                {
                    throw new Exception("Not saved in DB");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving image to database: {ex.Message}");
            }
        }

        private void SaveImageOnDevice()
        {
            var folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "ColorMorphImages");
            Directory.CreateDirectory(folderPath); // Ensure the folder is created

            var fileName = $"{_processedImage.Name}_{DateTime.Now:yyyyMMddHHmmss}.png";
            var filePath = Path.Combine(folderPath, fileName);

            try
            {
                File.WriteAllBytes(filePath, _processedImage.Data);
                Console.WriteLine($"Image saved to {filePath}");

                // Create and show notification
                var notification = new NotificationRequest
                {
                    Title = "Image Saved",
                    Description = $"Image saved to {filePath}",
                    NotificationId = 1000
                };
                LocalNotificationCenter.Current.Show(notification);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving image: {ex.Message}");
            }
        }

        private void SaveImage()
        {
            SaveImageOnDB();
            SaveImageOnDevice();
        }

        private void ResetImage()
        {
            ShowOriginal();
            _processedImage = new ImageEntity { Data = _originalImage.Data };
        }
        private void ShowOriginal()
        {
            ImageSource = ImageSource.FromStream(()=> new MemoryStream(_originalImage.Data));
        }

        private void ShowProccesed()
        {
            ImageSource = ImageSource.FromStream(() =>  new MemoryStream(_processedImage.Data));
        }

        private void ApplyImageProcessing(Func<byte[], byte[]> processingFunction)
        {
            var imageBytes = _processedImage.Data;
            _processedImage.Data = processingFunction(imageBytes);
            var processedBytes = _processedImage.Data;
            ImageSource = ImageSource.FromStream(() => new MemoryStream(processedBytes));
        }

    }
}