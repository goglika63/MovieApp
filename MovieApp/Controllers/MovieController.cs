using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MovieApp.Dto;
using MovieApp.Interfaces;
using MovieApp.Models;

namespace MovieApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : Controller
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;
        private readonly IReviewRepository _reviewRepository;

        public MovieController(IMovieRepository movieRepository, IMapper mapper, IReviewRepository reviewRepository)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;
            _reviewRepository = reviewRepository;
        }

        //GET ALL MOVIES
        [HttpGet]
        public IActionResult GetMovies()
        {
            var movies = _mapper.Map<List<MovieDto>>(_movieRepository.GetMovies());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(movies);
        }

        //GET MOVIE BY ID
        [HttpGet("{movieId}")]
        public IActionResult GetMovie(int movieId)
        {
            if (!_movieRepository.MovieExists(movieId))
                return NotFound();

            var movie = _mapper.Map<MovieDto>(_movieRepository.GetMovie(movieId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(movie);
        }

        //POST MOVIE
        [HttpPost]
        public IActionResult CreateMovie([FromQuery] int genreId, [FromBody] MovieDto movieCreate)
        {
            if (movieCreate == null)
                return BadRequest(ModelState);

            var movie = _movieRepository.GetMovies()
                .Where(m => m.Name.Trim().ToUpper() == movieCreate.Name.ToUpper())
                .FirstOrDefault();

            if (movie != null)
            {
                ModelState.AddModelError("", "Movie already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var movieMap = _mapper.Map<Movie>(movieCreate);

            if (!_movieRepository.MovieCreate(genreId, movieMap))
            {
                ModelState.AddModelError("", "Something went wrong while savin");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        //UPDATE MOVIE
        [HttpPut("{movieId}")]
        public IActionResult UpdateMovie(int movieId, [FromQuery] int genreId, [FromBody] MovieDto updatedMovie)
        {
            if (updatedMovie == null)
                return BadRequest(ModelState);

            if (movieId != updatedMovie.Id)
                return BadRequest(ModelState);

            if (!_movieRepository.MovieExists(movieId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var movieMap = _mapper.Map<Movie>(updatedMovie);

            if (!_movieRepository.UpdateMovie(genreId, movieMap))
            {
                ModelState.AddModelError("", "Something went wrong updating owner");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        //DELETE MOVIE
        [HttpDelete("{movieId}")]
        public IActionResult DeleteMovie(int movieId)
        {
            if (!_movieRepository.MovieExists(movieId))
            {
                return NotFound();
            }

            var reviewsToDelete = _reviewRepository.GetReviewsOfAMovie(movieId);
            var movieToDelete = _movieRepository.GetMovie(movieId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_reviewRepository.DeleteReviews(reviewsToDelete.ToList()))
            {
                ModelState.AddModelError("", "Something went wrong when deleting reviews");
            }

            if (!_movieRepository.DeleteMovie(movieToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting movie");
            }

            return NoContent();
        }
    }
}
