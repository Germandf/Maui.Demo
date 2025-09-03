namespace MauiXaml;

public partial class AboutPage : ContentPage
{
    public AboutPage()
    {
        InitializeComponent();
    }

    private void LearnMore_Clicked(object? sender, EventArgs e)
    {
        DisplayAlert("Learn More", "You clicked the Learn More button.", "OK");
        Launcher.Default.OpenAsync("https://aka.ms/maui");
    }
}