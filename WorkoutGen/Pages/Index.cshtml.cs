using Microsoft.AspNetCore.Mvc.RazorPages;
using WorkoutGen.Data.Session;

namespace WorkoutGen.Pages
{
    public class IndexModel : PageModel
    {
        public IndexModel()
        {
        }

        // Triggered on initial page load
        public void OnGet()
        {
            HttpContext.Session.ClearExerciseSession();
        }
    }
}
