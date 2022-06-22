namespace MovieApp.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public Movie Movie { get; set; }
    }
}
