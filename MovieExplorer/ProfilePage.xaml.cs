namespace MovieExplorer;

public partial class ProfilePage : ContentPage
{
	public ProfilePage()
	{
		InitializeComponent();
	}

    private async void Button_Clicked(object sender, EventArgs e)
    {
		string username = entryUsername.Text; // store the name entered 
		Preferences.Default.Set("username", username); // save the name in preferences
		await Shell.Current.GoToAsync("//MoviesMainPage"); // makes the mainPage root
    }
}