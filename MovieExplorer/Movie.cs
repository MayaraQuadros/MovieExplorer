using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieExplorer
{
    public class Movie
    {
        public string Title { get; set; }
        public int Year { get; set; }
        public string Genre { get; set; }
        public string Director { get; set; }
        public double Imdb { get; set; }
        public string Cover { get; set; }

        //constructor
        public Movie(string title, int year, String genre, string director, double imdb, string cover)
        {
            this.Title = title;
            this.Year = year;
            this.Genre = genre;
            this.Director = director;
            this.Imdb = imdb;
            this.Cover = cover;
                
        }

    }
}
