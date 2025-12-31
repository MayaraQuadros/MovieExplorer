using System.Threading.Tasks;

namespace MovieExplorer;

[QueryProperty(nameof(MovieProperty), "Movie")] //
public partial class MovieDetailPage : ContentPage
{
    private bool _isDownloading = false;

    public bool IsDownloading
    {
        get => _isDownloading;
        set
        {
            if (_isDownloading != value)
            {
                _isDownloading = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsNotDownloading));
            }
        }
    }
    public bool IsNotDownloading => !IsDownloading;


    public MovieDetailPage()
	{
		InitializeComponent();
		
	}

    public Movie MovieProperty { get; set; }
    private async void BackButton_Clicked(object sender, EventArgs e)
    {
		await Shell.Current.GoToAsync("..");
    }

    private bool _imgLoaded = false;
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (!_imgLoaded)
        {
            await Task.Delay(50);
            IsDownloading = true;
            BindingContext = MovieProperty;
            try
            {
                byte[] data = await App.HttpClient.GetByteArrayAsync(MovieProperty.Cover);

                ImageSource imgSrc = ImageSource.FromStream(() => new MemoryStream(data));
                ImageDisplay.Source = imgSrc;
            }
            catch (Exception ex)
            {
                //await DisplayAlert("Error Downloading Image", "Could not download Image", "OK");
                ImageDisplay.BackgroundColor = Colors.Black;
            }
            _imgLoaded = true;
            IsDownloading = false;
        }
    }
}