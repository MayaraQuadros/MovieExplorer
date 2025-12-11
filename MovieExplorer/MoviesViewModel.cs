using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace MovieExplorer
{
    internal class MoviesViewModel : INotifyPropertyChanged // implement Interface
    {
        private string _url;
        private Movie _selectedMovie;

        private ObservableCollection<Movie> _filteredMovies;

        private ObservableCollection<Movie> _movies;

        private int _listMovieSize;
        public ObservableCollection<Movie> Movies
        {
            get => _movies;
            set
            {
                _movies = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Movie> FilteredMovies
        {
            get => _filteredMovies;
            set
            {
                _filteredMovies = value;
                OnPropertyChanged();
            }
        }


        public int ListMovieSize
        {
            get { return _listMovieSize; }
            set
            {
                _listMovieSize = value;
                OnPropertyChanged();
            }
        }

        //constructor
        public MoviesViewModel()
        {
            Movies = new ObservableCollection<Movie>();
            FilteredMovies = new ObservableCollection<Movie>();
            
        }

        //overloade constructor
        public MoviesViewModel(string url) 
        {
      
            _url = url;
            Movies = new ObservableCollection<Movie>();
            FilteredMovies = new ObservableCollection<Movie>();

        }

        public void SortMovies(string sortBy)
        {
            switch(sortBy)
            {

            }
        }

   

        public Movie SelectedMovie {
            get
            { 
                return _selectedMovie;
            }
            set
            { 
                if (_selectedMovie != value)
                {
                    _selectedMovie = value;
                    OnPropertyChanged();
                }
            }
        }

       
        public async Task Favourite(Movie movie)
        {
            string filename = Path.Combine(FileSystem.Current.AppDataDirectory, "favourites.json");
            if (File.Exists(filename))
            {
                string jsonContents = JsonSerializer.Serialize(movie);
                using StreamWriter writer = new StreamWriter(filename, append: true);
                await writer.WriteAsync(jsonContents);
                    
            }
            else
            {
                using FileStream outputStream = File.Create(filename);
                string jsonContents = JsonSerializer.Serialize(movie);
                using StreamWriter writer = new StreamWriter(outputStream);
                await writer.WriteAsync(jsonContents);
            }
                
           
           
        }
        public bool IsLoaded { get; private set; } = false;

        public async Task DownloadMovies()
        {
            
            if (!IsLoaded)
            {
                //set up cache filename
                string filename = Path.Combine(FileSystem.Current.AppDataDirectory, "list_movies.json");
                
                //if file exists reads the content into observable colection
                if (File.Exists(filename))
                {
                    using FileStream inputStream = File.OpenRead(filename);
                    using StreamReader reader = new StreamReader(inputStream);
                    string contents = await reader.ReadToEndAsync();
                    List<Movie> _movies = JsonSerializer.Deserialize<List<Movie>>(contents); // create list of objects
                    foreach(var movie in _movies) // add one movie at a time into observable collection
                    {
                        
                        Movies.Add(movie);
                        FilteredMovies.Add(movie);
                        ListMovieSize = FilteredMovies.Count; //size of the list
                         
                    }                                                                         
                }
                else
                {
                    try 
                    {
                        //download from github
                        var response = await App.HttpClient.GetAsync(_url);
                        if (response != null && response.IsSuccessStatusCode)
                        {
                            string contents = await response.Content.ReadAsStringAsync();
                            List<Movie> _movies = JsonSerializer.Deserialize<List<Movie>>(contents); // create list of objects
                            foreach (var movie in _movies) // add one movie at a time into observable collection
                            {
                                Movies.Add(movie);
                                FilteredMovies.Add(movie);
                                ListMovieSize = FilteredMovies.Count; //size of the list

                            }
                            //save it to the device
                            using FileStream outputStream = File.Create(filename);
                            using StreamWriter writer = new StreamWriter(outputStream);
                            await writer.WriteAsync(contents);
                        }
                    }
                    catch
                    {

                    }
                }
                IsLoaded = true;
            }
              
        }
        
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
