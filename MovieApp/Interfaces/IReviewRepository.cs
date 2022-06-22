using MovieApp.Models;

namespace MovieApp.Interfaces
{
    public interface IReviewRepository
    {
        ICollection<Review> GetReviews();
        Review GetReview(int id);
        Review GetReview(string title);
        ICollection<Review> GetReviewsOfAMovie(int movieId);
        bool ReviewExists(int id);
        bool ReviewCreate(Review review);
        bool DeleteReview(Review review);
        bool DeleteReviews(List<Review> reviews);
        bool ReviewUpdate(Review review);
        bool Save();
    }
}
