namespace MovieExplorer;

public partial class FavouritePage : ContentPage
{
    MoviesViewModel viewModel;
    private MovieShared movieShared = new MovieShared();
    public FavouritePage()
	{
		InitializeComponent();
        viewModel = new MoviesViewModel();
        BindingContext = viewModel;

    }

    private async void BackButton_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private async void CollectionView_SelectionChangedFavourite(object sender, SelectionChangedEventArgs e)
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
        await viewModel.ReadFavourite();
    }

  

    private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Movie selectedMovie)
        {
            await movieShared.ShowMovieDetails(selectedMovie);
            ((CollectionView)sender).SelectedItem = null; // deselect the item in the collection view
        }
    }

}
