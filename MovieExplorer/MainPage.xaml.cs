

using System;
using System.IO;
using System.Net.NetworkInformation;
using System.Text.Json;
using System.Threading.Tasks;


namespace MovieExplorer
{
    public partial class MainPage : ContentPage
    {
        
        private MoviesViewModel viewModel;
        private MovieShared movieShared;
        private string _url;




        public MainPage(string url)
        {
            InitializeComponent();
            _url = url;
            viewModel = new MoviesViewModel(url);
            movieShared = new MovieShared();


            BindingContext = viewModel;

            applyTheme();
            


            //lblFile.Text = url; // debugger

        }

        
        

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

        public void applyTheme()
        {
            // TODO: Load settings using SettingsManager.LoadSettings()
            var settings = SettingsManager.LoadSettings();

            // TODO: Apply background color using dynamic resources
           
            if (!string.IsNullOrEmpty(settings.BackgroundColorFrame))
            {
                Application.Current.Resources["ThemeBGColorFrame"] = 
                Color.FromArgb(settings.BackgroundColorFrame);
            }
            if (!string.IsNullOrEmpty(settings.TextColor))
            {
                Application.Current.Resources["TextColor"] =
                Color.FromArgb(settings.TextColor);
            }
            if (!string.IsNullOrEmpty(settings.FavouriteIcon))
            {
                Application.Current.Resources["FavouriteIcon"] = settings.FavouriteIcon;
            }
            if (!string.IsNullOrEmpty(settings.BackgroundColor))
            {
                Application.Current.Resources["PageBackgroundColor"] =
                Color.FromArgb(settings.BackgroundColor);
            }

        }//end ApplyTheme


       
        //show the movie page details when a movie is selected
        private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Movie selectedMovie)
            {
               await movieShared.ShowMovieDetails(selectedMovie);
                ((CollectionView)sender).SelectedItem = null; // deselect the item in the collection view
                
            }
            

        }//end CollectionView_SelectionChanged



       

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            favouriteListBtn.Rotation = 0; //reset the rotation

            applyTheme();
            TextSizeChange();

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


            Application.Current.Resources["TextSize"] = settings.TextSize;
            Application.Current.Resources["TitleTextSize"] = settings.TextSize;

        }
            

        public async void favouriteMovieBtn_Clicked(object sender, EventArgs e)
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
           
            await favouriteListBtn.RotateTo(360, 500);
            await Task.Delay(50);
            await Shell.Current.GoToAsync(nameof(FavouritePage)); 
        }

        private async void searchBarEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            string word = searchBarEntry.Text.ToLower(); //get search text from user

            viewModel.FilteredMovies.Clear();//clear the observableColletion

            // search the list for the entry word
            for (int i = 0; i < viewModel.Movies.Count; i++)
            {
                var movie = viewModel.Movies[i];
                if (movie != null)
                {
                    //if the title contains the word, add to FilteredMoves observableCollection
                    if (movie.Title.ToLower().Contains(word) || movie.Genre.ToLower().Contains(word) || movie.Director.ToLower().Contains(word) || movie.Year.ToString().Contains(word) || movie.Imdb.ToString().Contains(word))
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
   
