using Microsoft.EntityFrameworkCore;
using MovieApp.Models;

namespace MovieApp.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genre { get; set; }
        public DbSet<MovieGenre> MovieGenre { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MovieGenre>()
                .HasKey(bc => new { bc.MovieId, bc.GenreId });
            modelBuilder.Entity<MovieGenre>()
                .HasOne(bc => bc.Movie)
                .WithMany(b => b.MovieGenre)
                .HasForeignKey(bc => bc.MovieId);
            modelBuilder.Entity<MovieGenre>()
                .HasOne(bc => bc.Genre)
                .WithMany(c => c.MovieGenre)
                .HasForeignKey(bc => bc.GenreId);
        }
    }
}
