#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorPagesMovie.Data;
using RazorPagesMovie.Models;

namespace RazorPagesMovie.Pages.Movies
{
    public class DeleteModel : PageModel
    {
        private readonly RazorPagesMovieContext _context;
        private readonly ILogger<DeleteModel> _logger;

        public DeleteModel(RazorPagesMovieContext context, ILogger<DeleteModel> logger)
        {
            _context = context;
            _logger = logger;
            _logger.LogDebug("Constructor, intialize logger DeleteModel");
        }

        [BindProperty]
        public Movie Movie { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            _logger.LogDebug("OnGetAsync");
            if (id == null)
            {
                _logger.LogInformation("id is null {MovieID}", id);
                return NotFound();
            }

            Movie = await _context.Movie.FirstOrDefaultAsync(m => m.ID == id);

            if (Movie == null)
            {
                _logger.LogInformation("not found movie {MovieID}", id);
                return NotFound();
            }

            _logger.LogInformation("found movie {MovieTitle} {MovieID}", Movie.Title, Movie.ID);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            _logger.LogDebug("OnPostAsync");
            if (id == null)
            {
                _logger.LogInformation("not found {MovieID}", id);
                return NotFound();
            }

            Movie = await _context.Movie.FindAsync(id);

            if (Movie != null)
            {
                _context.Movie.Remove(Movie);
                _logger.LogInformation("Deleted movie {MovieTitle} {MovieID}", Movie.Title, Movie.ID);
                await _context.SaveChangesAsync();
                _logger.LogDebug("Deleted movie, SaveChangesAsync {MovieTitle} {MovieID}", Movie.Title, Movie.ID);
            }
            else
            {
                _logger.LogInformation("not found movie {MovieID}", id);
            }

            return RedirectToPage("./Index");
        }
    }
}
