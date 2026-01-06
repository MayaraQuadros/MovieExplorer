using System.Threading.Tasks;

namespace MovieExplorer;

[QueryProperty(nameof(MovieProperty), "Movie")] //
public partial class MovieDetailPage : ContentPage
{
    private bool _isDownloading = false;
    private bool _soundOn = true;
    MoviesViewModel viewModel;

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
        viewModel = new MoviesViewModel();
		
	}

    public Movie MovieProperty { get; set; }
    private async void BackButton_Clicked(object sender, EventArgs e)
    {
        viewModel.Stop(); // stop the music
		await Shell.Current.GoToAsync(".."); // back to main
    }

   

    private bool _imgLoaded = false;
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await viewModel.openMusic(MovieProperty.Genre);
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

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        viewModel.Stop(); // stop the music when user uses the back top button
    }

    private void Sound_Clicked(object sender, EventArgs e)
    {
        if(_soundOn)
        {
            viewModel.Stop();
            Sound.Source = "volume_mute.png";
        }
        else
        {
            viewModel.Start();
            Sound.Source = "volume.png";
        }
        _soundOn = !_soundOn;

    }
}