using MovieApp.Data;
using MovieApp.Interfaces;
using MovieApp.Models;

namespace MovieApp.Repository
{
    public class GenreRepository : IGenreRepository
    {
        public readonly DataContext _context;
        public GenreRepository(DataContext context)
        {
            _context = context;
        }

        public ICollection<Genre> GetGenres()
        {
            return _context.Genre.OrderBy(G => G.Id).ToList();
        }

        public Genre GetGenre(int genreId)
        {
            return _context.Genre.Where(g => g.Id == genreId).FirstOrDefault();
        }

        public Genre GetGenre(string name)
        {
            return _context.Genre.Where(g => g.Name == name).FirstOrDefault();
        }

        public ICollection<Movie> GetMovieByGenre(int genreId)
        {
            return _context.MovieGenre.Where(g => g.GenreId == genreId).Select(m => m.Movie).ToList();
        }

        public bool GenreExists(int genreId)
        {
            return _context.Genre.Any(g=>g.Id==genreId);
        }

        public bool CreateGenre(Genre genre)
        {
            _context.Add(genre);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateGenre(Genre genre)
        {
            _context.Update(genre);
            return Save();
        }

        public bool DeleteGenre(Genre genre)
        {
            _context.Remove(genre);
            return Save();
        }
    }
}
