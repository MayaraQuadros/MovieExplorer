using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MovieExplorer
{
    internal class MoviesViewModel : INotifyPropertyChanged // implement Interface
    {
        private Movie _selectedMovie;

        private ObservableCollection<Movie> _movies;

        public ObservableCollection<Movie> Movies
        {
            get => _movies;
            set
            {
                _movies = value;
                OnPropertyChanged();
            }
        }




        public MoviesViewModel() 
        {
            Movies = new ObservableCollection<Movie>
            {
            };
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
                    Movies = JsonSerializer.Deserialize<ObservableCollection<Movie>>(contents);

                }
                else
                {
                    try 
                    {
                        //download from the raw folder
                        using var stream = await FileSystem.OpenAppPackageFileAsync("list_movies.json");
                        using var reader = new StreamReader(stream);
                        string contents = await reader.ReadToEndAsync(); // read all the content into the string
                        Movies = JsonSerializer.Deserialize<ObservableCollection<Movie>>(contents);

                        //save it to the device
                        using FileStream outputStream = File.Create(filename);
                        using StreamWriter writer = new StreamWriter(outputStream);
                        await writer.WriteAsync(contents);


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
