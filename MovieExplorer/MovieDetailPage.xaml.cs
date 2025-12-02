namespace MovieExplorer;

public partial class MovieDetailPage : ContentPage
{
	public MovieDetailPage()
	{
		InitializeComponent();
	}

    private async void BackButton_Clicked(object sender, EventArgs e)
    {
		await Shell.Current.GoToAsync("..");
    }

   
}