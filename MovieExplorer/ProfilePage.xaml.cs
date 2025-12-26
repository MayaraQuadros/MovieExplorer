namespace MovieExplorer;

public partial class ProfilePage : ContentPage
{
	public ProfilePage()
	{
		InitializeComponent();
	}

    private async void Button_Clicked(object sender, EventArgs e)
    {
		string username = entryUsername.Text;
		Preferences.Default.Set("username", username);
		await Shell.Current.GoToAsync("//MoviesMainPage"); // makes the mainPage root
    }
}