using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPagesMovie.Pages
{
    public class PrivacyModel : PageModel
    {
        private readonly ILogger<PrivacyModel> _logger;

        public PrivacyModel(ILogger<PrivacyModel> logger)
        {
            _logger = logger;
            _logger.LogDebug("Constructor, initialize logger PrivacyModel");
        }

        public void OnGet()
        {
            _logger.LogDebug("OnGet");
        }
    }
}