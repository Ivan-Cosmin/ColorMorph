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
                    var editVM = new EditVM(stream);
                    var editPage = new EditPage(editVM);
                    await App.Current.MainPage.Navigation.PushAsync(editPage);
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }

        private async void TakePhoto()
        {
            await App.Current.MainPage.DisplayAlert("Info", "Take Photo Command Executed", "OK");
        }
    }
}