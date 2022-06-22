using MovieApp.Models;

namespace MovieApp.Interfaces
{
    public interface IMovieRepository
    {
        ICollection<Movie> GetMovies();
        Movie GetMovie(int id);
        Movie GetMovie(string name);
        bool MovieExists(int movieId);
        bool MovieCreate(int genreId, Movie movie);
        bool UpdateMovie(int genreId, Movie movie);
        bool DeleteMovie(Movie movie);
        bool Save();
    }
}
