using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WorkoutGen.Data;
using WorkoutGen.Models;

namespace WorkoutGen.Pages.Exercises
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(WorkoutGen.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGet()
        {
            return Page();
        }

        [BindProperty]
        public UserExercise UserExercise { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.UserExercise.Add(UserExercise);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Account/Manage/MyAccount", new { area = "Identity" });
        }
    }
}
