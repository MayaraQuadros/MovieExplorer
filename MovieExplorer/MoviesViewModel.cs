using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieExplorer
{
    internal class MoviesViewModel : INotifyPropertyChanged // implement Interface
    {
        private Movie _selectedMovie;
        public ObservableCollection<Movie> Movies { get; set; }


       

        public MoviesViewModel() 
        {
            Movies = new ObservableCollection<Movie>
            {
            };
        }

        public void addMovies(List<Movie> moviesList)
        {
            for (int i = 0; i < moviesList.Count; i++) {

                Movies.Add(new Movie {Title = moviesList[i].Title, 
                                        Year = moviesList[i].Year,
                                        Genre = moviesList[i].Genre,
                                        Director = moviesList[i].Director,
                                        Imdb = moviesList[i].Imdb,
                                        Cover = moviesList[i].Cover });
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

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
