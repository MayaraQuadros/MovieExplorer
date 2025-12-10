

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
        private string url;




        public MainPage(string url)
        {
            InitializeComponent();
            this.url = url;
            lightTheme = Preferences.Default.Get("LightTheme", true); //set light theme as default
            viewModel = new MoviesViewModel(url);
            BindingContext = viewModel;
            CreateGrid();
           
        }

        //change theme
        private void ToggleThemeButton_Clicked(object sender, EventArgs e)
        {
            lightTheme = !lightTheme;
            applyTheme();
            Preferences.Default.Set("LightTheme", lightTheme);
        }

        private void CreateGrid()
        {
            if(!gridCreated)
            {
                var rows = (viewModel.ListMovieSize / 3); //set number of rowns considering 3 columns
                for (int i = 0; i < rows; i++)
                {
                    GridMovies.AddRowDefinition(new RowDefinition());
                    GridMovies.AddColumnDefinition(new ColumnDefinition());
                }

                for (int i = 0; i < rows; ++i)
                {
                    for (int j = 0; j < rows; ++j)
                    {
                        Border styledBorder = new Border
                        {
                            BackgroundColor = Colors.Red,
                            Stroke = Colors.Black,
                            StrokeThickness = 3

                        };

                        GridMovies.Add(styledBorder, j, i);
                    }
                }
                gridCreated = true;
            }
            
           
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
   
