#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorPagesMovie.Data;
using RazorPagesMovie.Models;

namespace RazorPagesMovie.Pages.Movies
{
    public class EditModel : PageModel
    {
        private readonly RazorPagesMovieContext _context;
        private readonly ILogger<EditModel> _logger;

        public EditModel(RazorPagesMovieContext context, ILogger<EditModel> logger)
        {
            _context = context;
            _logger = logger;
            _logger.LogDebug("Constructor, intialize logger EditModel");
        }

        [BindProperty]
        public Movie Movie { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            _logger.LogDebug("OnGetAsync");
            if (id == null)
            {
                _logger.LogInformation("Movie not found {MovieID}", id);
                return NotFound();
            }

            Movie = await _context.Movie.FirstOrDefaultAsync(m => m.ID == id);

            if (Movie == null)
            {
                _logger.LogInformation("Movie not found {MovieID}", id);
                return NotFound();
            }
            _logger.LogDebug("Movie found {MovieID}", id);
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            _logger.LogDebug("OnPostAsync");
            if (!ModelState.IsValid)
            {
                _logger.LogInformation("Model State is not valid");
                return Page();
            }

            _context.Attach(Movie).State = EntityState.Modified;

            try
            {
                _logger.LogInformation("Model State is not valid");
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!MovieExists(Movie.ID))
                {
                    _logger.LogError(ex, "Unable to save changes, not found {MovieId}", Movie.ID);
                    return NotFound();
                }
                else
                {
                    _logger.LogCritical(ex, "Unable to save changes {MovieId}", Movie.ID);
                    throw;
                }
            }
            _logger.LogDebug("Redirec to page Index");
            return RedirectToPage("./Index");
        }

        private bool MovieExists(int id)
        {
            var movieExists = _context.Movie.Any(e => e.ID == id);
            if(movieExists)
            {
                _logger.LogDebug("Movie Exists {MovieId}", id);
            }
            else
            {
                _logger.LogInformation("Movie Does Not Exists {MovieId}", id);
            }
            return movieExists;
        }
    }
}