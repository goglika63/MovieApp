using Microsoft.AspNetCore.Mvc;
using MovieApp.Data;
using MovieApp.Interfaces;
using MovieApp.Models;
using System.Linq;

namespace MovieApp.Repository
{
    public class MovieRepository : IMovieRepository
    {
        public readonly DataContext _context;
        public MovieRepository(DataContext context)
        {
            _context = context;
        }

        public bool DeleteMovie(Movie movie)
        {
            _context.Remove(movie);
            return Save();
        }

        public Movie GetMovie(int id)
        {
            return _context.Movies.Where(m => m.Id == id).FirstOrDefault();
        }

        public Movie GetMovie(string name)
        {
            return _context.Movies.Where(m => m.Name == name).FirstOrDefault();
        }

        public ICollection<Movie> GetMovies()
        {
            return _context.Movies.OrderBy(m => m.Id).ToList();
        }

        //POST MOVIE
        public bool MovieCreate(int genreId, Movie movie)
        {
            var movieGenreEntity=_context.Genre.Where(a=>a.Id==genreId).FirstOrDefault();

            var movieGenre = new MovieGenre()
            {
                Genre = movieGenreEntity,
                Movie = movie,
            };

            _context.Add(movieGenre);

            _context.Add(movie);
            return Save();
        }

        public bool MovieExists(int movieId)
        {
            return _context.Movies.Any(m=> m.Id == movieId);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateMovie(int genreId, Movie movie)
        {
            _context.Update(movie);
            return Save();
        }
    }
}
