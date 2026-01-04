
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieExplorer
{
    class MovieShared
    {
        private MoviesViewModel viewModel = new MoviesViewModel();

        public async Task ShowMovieDetails(Movie selectedMovie)
        {
            var parameters = new Dictionary<string, object> // store the selected movie in a dictionary
                {
                    {"Movie", selectedMovie}
                };

            await Shell.Current.GoToAsync(nameof(MovieDetailPage), parameters); // send the dictionary to the MovieDetailPage
            
        }

        public void SearchBar(string word)
        {
            
            
        }
    }
}
