namespace MovieExplorer
{
    public partial class AppShell : Shell
    {

        private bool _firstTime = true;
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(MovieDetailPage), typeof(MovieDetailPage)); // register the route
            Routing.RegisterRoute(nameof(FavouritePage), typeof(FavouritePage));
            Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (_firstTime)
            {
                _firstTime = false;
                await Shell.Current.GoToAsync(nameof(ProfilePage));
            }
        }
    }
}
