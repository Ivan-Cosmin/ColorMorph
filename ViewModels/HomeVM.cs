using Microsoft.Maui.Controls;
using ColorMorph.Views;
using System.IO;
using System.Windows.Input;

namespace ColorMorph.ViewModels
{
    internal class HomeVM : BaseVM
    {
        public ICommand OpenImageCommand { get; }
        public ICommand TakePhotoCommand { get; }

        public HomeVM()
        {
            OpenImageCommand = new Command(OpenImage);
            TakePhotoCommand = new Command(TakePhoto);
        }

        private async void OpenImage()
        {
            try
            {
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = "Please select an image",
                    FileTypes = FilePickerFileType.Images
                });

                if (result != null)
                {
                    var stream = await result.OpenReadAsync();
                    using var memoryStream = new MemoryStream();
                    await stream.CopyToAsync(memoryStream);
                    var imageData = memoryStream.ToArray();

                    // Salvăm imaginea în baza de date
                    var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "images.db");
                    var dbService = new DatabaseService(dbPath);
                    var imageId = dbService.SaveImage(result.FileName, imageData);

                    // Creăm ViewModel-ul pentru pagina de editare
                    var savedImage = dbService.GetImage(imageId);
                    var editVM = new EditVM(new MemoryStream(savedImage.Data), savedImage);
                    var editPage = new EditPage(editVM);

                    // Navigăm către pagina de editare
                    var mainPage = App.Current?.Windows[0]?.Page;
                    if (mainPage != null)
                    {
                        await mainPage.Navigation.PushAsync(editPage);
                    }
                    else
                    {
                        await App.Current?.Windows[0]?.Page?.DisplayAlert("Error", "Main page is not available", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await App.Current?.Windows[0]?.Page?.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }


        private async void TakePhoto()
        {
            await App.Current?.Windows[0]?.Page?.DisplayAlert("Info", "Take Photo Command Executed", "OK");
        }
    }
}