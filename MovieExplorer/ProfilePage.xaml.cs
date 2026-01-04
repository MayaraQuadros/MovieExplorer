using System.Threading.Tasks;

namespace MovieExplorer;

public partial class ProfilePage : ContentPage
{
	public ProfilePage()
	{
		InitializeComponent();
        lblName.Opacity = 0;
        entryUsername.Opacity = 0;
        borderEntry.Opacity = 0;
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
		string username = entryUsername.Text; // store the name entered 
		Preferences.Default.Set("username", username); // save the name in preferences
		await Shell.Current.GoToAsync("//MoviesMainPage"); // makes the mainPage root
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        
        await Task.Delay(7000);
        lblName.Opacity = 1;
        entryUsername.Opacity = 1;
        borderEntry.Opacity = 1;
        
        startBtn.IsEnabled = true;
    }
}