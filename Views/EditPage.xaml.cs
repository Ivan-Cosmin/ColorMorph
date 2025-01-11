using Microsoft.Maui.Controls;
using ColorMorph.ViewModels;

namespace ColorMorph.Views
{
    public partial class EditPage : ContentPage
    {
        public EditPage(EditVM viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}