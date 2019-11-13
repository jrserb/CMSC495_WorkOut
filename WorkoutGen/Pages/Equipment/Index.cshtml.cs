using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WorkoutGen.Models;

namespace WorkoutGen.Pages.Equipment
{
    public class IndexModel : PageModel
    {
        private readonly WorkoutGenContext _context;

        public IndexModel(WorkoutGenContext context)
        {
            _context = context;
        }

        // Binding properties allows them to be passed to the razor page model
        // So we can access it during requests and responses
        [BindProperty]
        public List<Models.MuscleGroup> muscleGroups { get; set; }
        public int exerciseCount { get; set; }
        public SelectList Options_Equipment { get; set; }

        public IActionResult OnGet()
        {
            TempData.Keep("muscleGroupIds");
            int[] muscleGroupIds = (int[])TempData["muscleGroupIds"];

            if (muscleGroupIds.Length > 0)
            {
                // If full body was selected then get all the equipment
                if (muscleGroupIds[0] == 6)
                {
                    Options_Equipment = new SelectList(_context.Equipment, "Id", "Name");
                    exerciseCount = _context.Exercise.Count();
                }
                else
                {
                    // First get distinct list of exercise ids associated with the muscle group ids
                    List<int> exerciseIds = _context.ExerciseMuscleGroup
                                            .Where(m => muscleGroupIds.Contains(m.MuscleGroupId))
                                            .Select(r => r.ExerciseId)
                                            .Distinct()
                                            .ToList();

                    // Start with 0 because they have no equipment selected yet
                    exerciseCount = 0;

                    var equipment = (from eq in _context.Equipment
                                    join ee in _context.ExerciseEquipment on eq.Id equals ee.EquipmentId
                                    where exerciseIds.Contains(ee.ExerciseId)
                                    select eq).Distinct();

                    Options_Equipment = new SelectList(equipment, "Id", "Name");
                }

                // Return the selected muscle groups to display at the top
                muscleGroups = _context.MuscleGroup
                               .Where(m => muscleGroupIds.Contains(m.Id))
                               .ToList();

                return Page();
            }

            return RedirectToPage("/MuscleGroup/Index");
        }

        public JsonResult OnPostUpdateExerciseCount(int[] equipmentIds)
        {
            // Get distinct count of exercises associated with the selected equipment ids
            int count = _context.ExerciseEquipment
                        .Where(m => equipmentIds.Contains(m.EquipmentId))
                        .Distinct()
                        .Count();

            return new JsonResult(count);
        }
    }
}