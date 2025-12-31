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
        private int _listMovieSize;
        private Movie _selectedMovie;
        
       

        private ObservableCollection<Movie> _filteredMovies;
        private ObservableCollection<Movie> _movies;
        private ObservableCollection<Movie> _favouriteMovies;

      
        public ObservableCollection<Movie> Movies
        {
            get { return _movies; }
            set
            {
                _movies = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Movie> FilteredMovies
        {
            get { return _filteredMovies; }
            set
            {
                _filteredMovies = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Movie> FavouriteMovies
        {
            get { return _favouriteMovies; }
            set
            {
                _favouriteMovies = value;
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
            FavouriteMovies = new ObservableCollection<Movie>();

            
        }

        //overloade constructor
        public MoviesViewModel(string url) 
        {
      
            _url = url;
            Movies = new ObservableCollection<Movie>();
            FilteredMovies = new ObservableCollection<Movie>();
            FavouriteMovies = new ObservableCollection<Movie>();

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

        //debugger
        private string _message;
        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                OnPropertyChanged();
            }
        }

        public async Task Favourite(Movie movie)
        {
            
            string filename = Path.Combine(FileSystem.Current.AppDataDirectory, "favourites.json");
            //if file exists reads the content 
            if (File.Exists(filename))
            {
                string contents = ReadFile(filename);//read all the content

                if (!contents.Contains(movie.Title)) // only appends if the movie is not in the list
                {
                    //serialize with indentation
                    string jsonContents = "," + JsonSerializer.Serialize(movie, new JsonSerializerOptions { WriteIndented = true });


                    contents += jsonContents; // append the new favourite

                    using StreamWriter writer = new StreamWriter(filename);
                    await writer.WriteAsync(contents); //write to file

                    //Message = jsonContents; debugger
                }
            }
            else 
            {
                string jsonContents = JsonSerializer.Serialize(movie, new JsonSerializerOptions { WriteIndented = true });
                //string contents = "[" + jsonContents + "]";
                using StreamWriter writer = new StreamWriter(filename);
                await writer.WriteAsync(jsonContents);
            }
        }

        private string ReadFile(string filename)
        {
            using FileStream inputStream = File.OpenRead(filename);
            using StreamReader reader = new StreamReader(inputStream);
            string contents = reader.ReadToEnd();
            return contents;
        }

        public async Task ReadFavourite()
        {
            string filename = Path.Combine(FileSystem.Current.AppDataDirectory, "favourites.json");
            //if file exists reads the content into observable colection
            if (File.Exists(filename))
            {
                string contents = ReadFile(filename);
                contents = "[" + contents + "]";
                //Message = contents; debugger

                FavouriteMovies = JsonSerializer.Deserialize<ObservableCollection<Movie>>(contents);
                ListMovieSize = FavouriteMovies.Count; //size of the list
                
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
                    string contents = ReadFile(filename);
                    FilteredMovies = JsonSerializer.Deserialize<ObservableCollection<Movie>>(contents);
                    Movies = JsonSerializer.Deserialize<ObservableCollection<Movie>>(contents);

                    /*List<Movie> _movies = JsonSerializer.Deserialize<List<Movie>>(contents); // create list of objects
                    foreach(var movie in _movies) // add one movie at a time into observable collection
                    {
                        
                        Movies.Add(movie);
                        FilteredMovies.Add(movie);
                    }*/
                    
                    ListMovieSize = FilteredMovies.Count; //size of the list
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
                            FilteredMovies = JsonSerializer.Deserialize<ObservableCollection<Movie>>(contents);
                            Movies = JsonSerializer.Deserialize<ObservableCollection<Movie>>(contents);

                            /*List<Movie> _movies = JsonSerializer.Deserialize<List<Movie>>(contents); // create list of objects
                            foreach (var movie in _movies) // add one movie at a time into observable collection
                            {
                                Movies.Add(movie);
                                FilteredMovies.Add(movie);
                                

                            }*/
                            
                            ListMovieSize = FilteredMovies.Count; //size of the list
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
