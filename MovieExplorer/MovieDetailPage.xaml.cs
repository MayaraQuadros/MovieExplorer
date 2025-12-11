using System.Threading.Tasks;

namespace MovieExplorer;

[QueryProperty(nameof(MovieProperty), "Movie")] //
public partial class MovieDetailPage : ContentPage
{

	
	public MovieDetailPage()
	{
		InitializeComponent();
		BindingContext = MovieProperty;
	}

    public Movie MovieProperty { get; set; }
    private async void BackButton_Clicked(object sender, EventArgs e)
    {
		await Shell.Current.GoToAsync("..");
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
         BindingContext = MovieProperty;
        await Task.Delay(50);
    }




}