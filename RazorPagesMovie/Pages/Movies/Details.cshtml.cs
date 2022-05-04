#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorPagesMovie.Data;
using RazorPagesMovie.Models;

namespace RazorPagesMovie.Pages.Movies
{
    public class DetailsModel : PageModel
    {
        private readonly RazorPagesMovieContext _context;
        private readonly ILogger<DetailsModel> _logger;

        public DetailsModel(RazorPagesMovieContext context, ILogger<DetailsModel> logger)
        {
            _context = context;
            _logger = logger;
            _logger.LogDebug("Constructor, initialize Logger DetailsModel");
        }

        public Movie Movie { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            _logger.LogDebug("OnGetAsync");
            if (id == null)
            {
                _logger.LogInformation("id is null");
                return NotFound();
            }

            Movie = await _context.Movie.FirstOrDefaultAsync(m => m.ID == id);

            if (Movie == null)
            {
                _logger.LogInformation("movie not found {MovieId}", id);
                return NotFound();
            }
            
            _logger.LogDebug("Movie found {MovieTitle} {MovieID}", Movie.Title, Movie.ID);
            return Page();
        }
    }
}
