namespace ColorMorph.Views;
using ColorMorph.ViewModels;
public partial class HomePage : ContentPage
{
    public HomePage()
    {
        InitializeComponent();
        BindingContext = new HomeVM();
    }
}

