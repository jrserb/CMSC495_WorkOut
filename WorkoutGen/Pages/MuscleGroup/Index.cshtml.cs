using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using WorkoutGen.Models;

namespace WorkoutGen.Pages.MuscleGroup
{
    public class IndexModel : PageModel
    {
        private readonly WorkoutGenContext _context;

        public IndexModel(WorkoutGenContext context)
        {
            _context = context;
        }

        [BindProperty]
        [MustHaveOneItem(ErrorMessage = "You must select an option")]
        public int[] muscleGroupIds { get; set; }

        public SelectList Options_MuscleGroups { get; set; }

        public void OnGet()
        {
            // Easy way to render the drop down when page first loads
            // Populate binded property
            Options_MuscleGroups = new SelectList(_context.MuscleGroup, "Id", "Name");
        }

        public IActionResult OnPost(int[] muscleGroupIds)
        {        
            if (!ModelState.IsValid) {

                Options_MuscleGroups = new SelectList(_context.MuscleGroup, "Id", "Name");
                return Page();
            }

            TempData["muscleGroupIds"] = muscleGroupIds;
            return RedirectToPage("/Equipment/Index");
        }

        public class MustHaveOneItem : ValidationAttribute
        {
            public override bool IsValid(object value)
            {
                var list = value as int[];
                if (list.Length > 0)
                {
                    return true;
                }
                return false;
            }
        }
    }
}