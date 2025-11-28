

using System.IO;
using System.Net.NetworkInformation;
using System.Text.Json;

namespace MovieExplorer
{
    public partial class MainPage : ContentPage
    {
        private bool lightTheme;
        //Movie[] _moviesObjects = new Movie[10];
        List<Movie> movieList; // why do i need to create this
        private MoviesViewModel viewModel; //// why do i need to create this




        public MainPage()
        {
            InitializeComponent();
            lightTheme = Preferences.Default.Get("LightTheme", true); //set light theme as default
            viewModel = new MoviesViewModel();
            BindingContext = viewModel;
        }

        //change theme
        private void ToggleThemeButton_Clicked(object sender, EventArgs e)
        {
            lightTheme = !lightTheme;
            applyTheme();
            Preferences.Default.Set("LightTheme", lightTheme);
        }


        private void applyTheme()
        {
            if (lightTheme)
            {
                Resources["PageBackgroundColor"] = Resources["LightBackgroundColor"];
                ToggleThemeButton.TextColor = Colors.Black;
                lblMoviesList.TextColor = Colors.Black;
                ToggleThemeButton.BackgroundColor = Colors.BlueViolet;


            }
            else
            {
                Resources["PageBackgroundColor"] = Resources["DarkBackgroundColor"];
                ToggleThemeButton.TextColor = Colors.LightGray;
                lblMoviesList.TextColor = Colors.LightGrey;
                ToggleThemeButton.BackgroundColor = Colors.Teal;
            }
        }


        private void SearchBar_SearchButtonPressed(object sender, EventArgs e)
        {

        }

        private async void btnReadFile_Clicked(object sender, EventArgs e)
        {
            //open and try to read the file
            string jsonstring = "";
            try
            {
                using var stream = await FileSystem.OpenAppPackageFileAsync("list_movies.json");
                using var reader = new StreamReader(stream);
                jsonstring = await reader.ReadToEndAsync(); // read all the content into the string
                List<Movie> _movies = JsonSerializer.Deserialize<List<Movie>>(jsonstring); // create list of objects 
                viewModel.addMovies(_movies); // add books to observable collection
               

            }
            catch
            {
                await DisplayAlert("Error", "Could not read file", "OK");
                return;
            }
   
        }

    }
}
   
