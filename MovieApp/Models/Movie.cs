namespace MovieApp.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Year { get; set; }
        public ICollection<MovieGenre> MovieGenre { get; set; }
        public string Country { get; set; }
        public string Income { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }
}
