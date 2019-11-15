using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WorkoutGen.Models;
using WorkoutGen.Data.Services.Exercise;
using System.Threading.Tasks;

namespace WorkoutGen.Pages.Exercises
{
    public class IndexModel : PageModel
    {
        private readonly IExerciseService _exerciseDb;

        public IndexModel(IExerciseService exerciseDb)
        {
            _exerciseDb = exerciseDb;
        }

        [BindProperty]
        public IEnumerable<Exercise> Exercises { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Exercises = await _exerciseDb.GetExercises();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int[] equipmentIds)
        {
            Exercises = await _exerciseDb.GetExercisesFromRequiredEquipment(equipmentIds);
            return Page();
        }
    }
}
