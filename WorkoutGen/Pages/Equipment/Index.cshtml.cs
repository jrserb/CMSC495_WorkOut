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
        public int[] muscleGroupIds { get; set; }
        public List<Models.MuscleGroup> muscleGroups { get; set; }
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
                }
                else
                {
                    // First get distinct list of exercise ids associated with the muscle group ids
                    List<int> exerciseIds = _context.ExerciseMuscleGroup
                                            .Where(m => muscleGroupIds.Contains(m.MuscleGroupId))
                                            .Select(r => r.ExerciseId)
                                            .Distinct()
                                            .ToList();

                    var equipment = (from eq in _context.Equipment
                                    join ee in _context.ExerciseEquipment on eq.Id equals ee.EquipmentId
                                    where exerciseIds.Contains(ee.ExerciseId)
                                    select eq).Distinct();

                    Options_Equipment = new SelectList(equipment, "Id", "Name");
                }

                muscleGroups = _context.MuscleGroup
                                .Where(m => muscleGroupIds.Contains(m.Id))
                                .ToList();

                return Page();
            }

            return RedirectToPage("/MuscleGroup/Index");
        }

        public void OnPost()
        {
            
        }
    }
}