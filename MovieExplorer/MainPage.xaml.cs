

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
        private MoviesViewModel viewModel; 




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

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            //checks if there is viewModel object and if the file was already downloade
            if (viewModel != null && !viewModel.IsLoaded)
            {
                await Task.Delay(50);
                await viewModel.DownloadMovies();
              
            }
        }
    }
}
   
