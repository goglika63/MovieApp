using MovieApp.Models;

namespace MovieApp.Interfaces
{
    public interface IGenreRepository
    {
        ICollection<Genre> GetGenres();
        Genre GetGenre(int id);
        Genre GetGenre(string name);
        ICollection<Movie> GetMovieByGenre(int genreId);
        bool CreateGenre(Genre genre);
        bool GenreExists(int genreId);
        bool UpdateGenre(Genre genre);
        bool DeleteGenre(Genre genre);
        bool Save();
    }
}
