#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagesMovie.Data;
using RazorPagesMovie.Models;

namespace RazorPagesMovie.Pages.Movies
{
    public class CreateModel : PageModel
    {
        private readonly RazorPagesMovieContext _context;
        private readonly ILogger<CreateModel> _logger;

        public CreateModel(RazorPagesMovieContext context, ILogger<CreateModel> logger)
        {
            _context = context;
            _logger = logger;
            _logger.LogDebug("Constructor, initialize logger CreateModel");
        }

        public IActionResult OnGet()
        {
            _logger.LogDebug("OnGet");
            return Page();
        }

        [BindProperty]
        public Movie Movie { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            _logger.LogDebug("OnPostAsyc");
            if (!ModelState.IsValid)
            {
                _logger.LogInformation("ModelState is not valid {ModelState}", ModelState);
                return Page();
            }

            _context.Movie.Add(Movie);
            
            await _context.SaveChangesAsync();
            _logger.LogInformation("Movie Created: {MovieTitle} {MovieId} {MovieGenre} {MovieRating} {MovieReleaseDate}", Movie.Title, Movie.ID, Movie.Genre, Movie.Rating, Movie.ReleaseDate);
            
            _logger.LogDebug("Redirect to page ./Index");
            return RedirectToPage("./Index");
        }
    }
}
