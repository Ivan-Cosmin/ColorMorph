using Microsoft.Maui.Controls;
using ColorMorph.Views;
using System.IO;
using System.Windows.Input;

namespace ColorMorph.ViewModels
{
    internal class HomeVM : BaseVM
    {
        public ICommand OpenImageCommand { get; }
        public ICommand GetRandomImageCommand { get; }

        public HomeVM()
        {
            OpenImageCommand = new Command(OpenImage);
            GetRandomImageCommand = new Command(async () => await FetchRandomImageAsync());

        }

        private async void OpenEditor(Stream stream, String nameImage)
        {
            try
            {
                using var memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);
                var imageData = memoryStream.ToArray();

                var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "images.db");
                var dbService = new DatabaseService(dbPath);
                var imageId = dbService.SaveImage(nameImage, imageData);

                var savedImage = dbService.GetImage(imageId);
                var editVM = new EditVM(new MemoryStream(savedImage.Data), savedImage);
                var editPage = new EditPage(editVM);

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
            catch (Exception ex)
            {
                await App.Current?.Windows[0]?.Page?.DisplayAlert("Error", $"Failed to fetch image: {ex.Message}", "OK");
            }
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
                    OpenEditor(stream, result.FileName);
                }
            }
            catch (Exception ex)
            {
                await App.Current?.Windows[0]?.Page?.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }

        private async Task FetchRandomImageAsync()
        {
            try
            {
                string imageUrl = "https://picsum.photos/500";
                var httpClient = new HttpClient();

                var response = await httpClient.GetAsync(imageUrl);
                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();

                OpenEditor(stream, "RandomImage.jpg");
            }
            catch (Exception ex)
            {
                await App.Current?.Windows[0]?.Page?.DisplayAlert("Error", $"Failed to fetch image: {ex.Message}", "OK");
            }
        }

    }
}