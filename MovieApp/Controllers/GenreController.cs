using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MovieApp.Dto;
using MovieApp.Interfaces;
using MovieApp.Models;

namespace MovieApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenreController : Controller
    {
        private readonly IGenreRepository _genreRespository;
        private readonly IMapper _mapper;

        public GenreController(IGenreRepository genreRepository, IMapper mapper)
        {
            _genreRespository = genreRepository;
            _mapper = mapper;
        }

        //GET ALL GENRE
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Genre>))]
        public IActionResult GetGenres()
        {
            var genres = _mapper.Map<List<GenreDto>>(_genreRespository.GetGenres());

            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(genres);
        }

        //GET GENRE BY ID
        [HttpGet("{genreId}")]
        public IActionResult GetGenre(int genreId)
        {
            if (!_genreRespository.GenreExists(genreId))
                return NotFound();

            var genre = _mapper.Map<GenreDto>(_genreRespository.GetGenre(genreId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(genre);
        }

        //GET MOVIE BY GENRE
        [HttpGet("movie/{genreId}")]
        public IActionResult GetMovieByGenreId(int genreId)
        {
            var movies = _mapper.Map<List<MovieDto>>(_genreRespository.GetMovieByGenre(genreId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(movies);
        }

        //POST GENRE
        [HttpPost]
        public IActionResult CreateGenre([FromBody] GenreDto genreCreate)
        {
            if (genreCreate == null)
                return BadRequest(ModelState);

            var genre = _genreRespository.GetGenres()
                .Where(g => g.Name.Trim().ToUpper() == genreCreate.Name.ToUpper())
                .FirstOrDefault();

            if (genre != null)
            {
                ModelState.AddModelError("", "Genre already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var genreMap = _mapper.Map<Genre>(genreCreate);

            if (!_genreRespository.CreateGenre(genreMap))
            {
                ModelState.AddModelError("", "Something went wrong while savin");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        //UPDATE GENRE
        [HttpPut("{genreId}")]
        public IActionResult UpdateGenre(int genreId, [FromBody] GenreDto updatedGenre)
        {
            if (updatedGenre == null)
                return BadRequest(ModelState);

            if (genreId != updatedGenre.Id)
                return BadRequest(ModelState);

            if (!_genreRespository.GenreExists(genreId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var genreMap = _mapper.Map<Genre>(updatedGenre);

            if (!_genreRespository.UpdateGenre(genreMap))
            {
                ModelState.AddModelError("", "Something went wrong updating genre");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        //DELETE GENRE
        [HttpDelete("{genreId}")]
        public IActionResult DeleteGenre(int genreId)
        {
            if (!_genreRespository.GenreExists(genreId))
            {
                return NotFound();
            }

            var genreToDelete = _genreRespository.GetGenre(genreId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_genreRespository.DeleteGenre(genreToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting genre");
            }

            return NoContent();
        }
    }
}
