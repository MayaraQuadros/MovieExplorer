

using System;
using System.IO;
using System.Net.NetworkInformation;
using System.Text.Json;
using System.Threading.Tasks;

namespace MovieExplorer
{
    public partial class MainPage : ContentPage
    {
        private bool lightTheme;
        private bool gridCreated = false;
        private MoviesViewModel viewModel;
        private string _url;




        public MainPage(string url)
        {
            InitializeComponent();
            _url = url;
            lightTheme = Preferences.Default.Get("LightTheme", true); //set light theme as default
            viewModel = new MoviesViewModel(url);
            
            BindingContext = viewModel;
            

            //lblFile.Text = url; // debugger

        }

        //change theme
        private void ToggleThemeButton_Clicked(object sender, EventArgs e)
        {
            lightTheme = !lightTheme;
            applyTheme();
            Preferences.Default.Set("LightTheme", lightTheme);
        }//end ToggleThemeButton_Clicked

        private async Task Greetings()
        {
            string username = Preferences.Default.Get("username", ""); //get the name saved in preferences
            lblUsername.Text = $"Hi {username}! Are you ready?";
            await Task.Delay(3000);
            await lblUsername.FadeTo(0, 1000);
            lblUsername.Text = "Movies";
            await Task.Delay(50);
            lblUsername.ScaleTo(1.5, 500);
            await lblUsername.FadeTo(1, 500);
            
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
        }//end ApplyTheme


       

        private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Movie selectedMovie)
            {
                var parameters = new Dictionary<string, object> // store the selected movie in a dictionary
                {
                    {"Movie", selectedMovie}
                };

                await Shell.Current.GoToAsync(nameof(MovieDetailPage), parameters); // send the dictionary to the MovieDetailPage
                ((CollectionView)sender).SelectedItem = null; // deselect the item in the collection view
            }

        }//end CollectionView_SelectionChanged



       

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            //checks if there is viewModel object and if the file was already downloade
            if (viewModel != null && !viewModel.IsLoaded)
            {
                await Greetings();

                await Task.Delay(50);
                await viewModel.DownloadMovies();
                
                


            }
        }//end OnAppearing

        private void TextSizeChange()
        {
            // Load settings using SettingsManager.LoadSettings()
            var settings = SettingsManager.LoadSettings();

            

        }

        private async void favouriteMovieBtn_Clicked(object sender, EventArgs e)
        {
           
           if(sender is ImageButton btn && btn.BindingContext is Movie selectedMovie)
            {
                viewModel.Favourite(selectedMovie);

                await btn.ScaleTo(2, 300);
                await btn.ScaleTo(1, 300);

            }
        
        }

        private async void favouriteListBtn_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(FavouritePage)); 
        }

        private void searchBarEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchEntry = searchBarEntry.Text.ToLower(); //get search text from user
            
            viewModel.FilteredMovies.Clear();//clear the observableColletion

            // search the list for the entry word
            for (int i = 0; i < viewModel.Movies.Count; i++)
            {
                var movie = viewModel.Movies[i];
                if (movie != null)
                {
                    //if the title contains the word, add to FilteredMoves observableCollection
                    if (movie.Title.ToLower().Contains(searchEntry) || movie.Genre.ToLower().Contains(searchEntry) || movie.Director.ToLower().Contains(searchEntry) || movie.Year.ToString().Contains(searchEntry) || movie.Imdb.ToString().Contains(searchEntry))
                    {
                        viewModel.FilteredMovies.Add(movie);

                    }
                }
                else
                {
                    viewModel.FilteredMovies = viewModel.Movies; //if the entry field is empty, show all movie again
                }
            }
        }//end SearchBarEntry

        private async void OnSettings_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(SettingsPage));
        }
    }
}
   
