using ColorMorph.ViewModels;
using Microsoft.Maui.Controls;

namespace ColorMorph.Views
{
    public partial class HistoryPage : ContentPage
    {
        private HistoryVM _viewModel;

        public HistoryPage()
        {
            InitializeComponent();
            _viewModel = BindingContext as HistoryVM;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel?.LoadImages();
        }
    }
}