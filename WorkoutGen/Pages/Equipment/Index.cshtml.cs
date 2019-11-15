using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WorkoutGen.Data.Services.Equipment;
using WorkoutGen.Data.Services.Exercise;
using WorkoutGen.Data.Services.MuscleGroup;

namespace WorkoutGen.Pages.Equipment
{
    public class IndexModel : PageModel
    {
        private readonly IExerciseService _exerciseDb;
        private readonly IEquipmentService _equipmentDb;
        private readonly IMuscleGroupService _muscleGroupDb;

        public IndexModel(IExerciseService exerciseDb, 
            IEquipmentService equipmentDb, 
            IMuscleGroupService muscleGroupDb)
        {
            _exerciseDb = exerciseDb;
            _equipmentDb = equipmentDb;
            _muscleGroupDb = muscleGroupDb;
        }

        // Binding properties allows them to be passed to the razor page model
        // So we can access it during requests and responses
        [BindProperty]
        public IEnumerable<Models.MuscleGroup> MuscleGroups { get; set; }
        public int ExerciseCount { get; set; }
        public int[] equipmentIds { get; set; }
        public SelectList Options_Equipment { get; set; }

        public IActionResult OnGet()
        {
            //TempData.Keep("muscleGroupIds");
            //int[] muscleGroupIds = (int[])TempData["muscleGroupIds"];

            //if (muscleGroupIds.Length > 0)
            //{
            //    // If full body was selected then get all the equipment
            //    if (muscleGroupIds[0] == 6)
            //    {
            //        Options_Equipment = new SelectList(_context.Equipment, "Id", "Name");
            //        exerciseCount = _context.Exercise.Count();
            //    }
            //    else
            //    {
            //        // First get distinct list of exercise ids associated with the muscle group ids
            //        List<int> exerciseIds = _context.ExerciseMuscleGroup
            //                                .Where(m => muscleGroupIds.Contains(m.MuscleGroupId))
            //                                .Select(r => r.ExerciseId)
            //                                .Distinct()
            //                                .ToList();

            //        // Start with 0 because they have no equipment selected yet
            //        exerciseCount = 0;

            //        var equipment = (from eq in _context.Equipment
            //                         join ee in _context.ExerciseEquipment on eq.Id equals ee.EquipmentId
            //                         where exerciseIds.Contains(ee.ExerciseId)
            //                         select eq).Distinct();

            //        Options_Equipment = new SelectList(equipment, "Id", "Name");
            //    }

            //    // Return the selected muscle groups to display at the top
            //    muscleGroups = _context.MuscleGroup
            //                   .Where(m => muscleGroupIds.Contains(m.Id))
            //                   .ToList();

            //return Page();
            //}

            return RedirectToPage("/MuscleGroup/Index");
        }

        public async Task<IActionResult> OnPostAsync(int[] muscleGroupIds)
        {
            MuscleGroups = await _muscleGroupDb.GetMuscleGroups(muscleGroupIds);
            IEnumerable <Models.Equipment> equipment;
            ExerciseCount = 0;

            if (muscleGroupIds[0] == 6)
            {
                equipment = await _equipmentDb.GetEquipment();
            }
            else
            {
                int[] exerciseIds = await _exerciseDb.GetExerciseIdsFromMuscleGroups(muscleGroupIds);
                int[] equipmentIds = await _equipmentDb.GetEquipmentIdsFromExercises(exerciseIds);
                int[] alternateEquipmentIds = await _equipmentDb.GetAlternateEquipmentIdsFromEquipment(equipmentIds);

                // Join the arrays of equipment and get distinct set
                int[] fullEquipmentIds = equipmentIds.Concat(alternateEquipmentIds).Distinct().ToArray();

                // Get the equipment
                equipment = await _equipmentDb.GetEquipment(fullEquipmentIds);
            }
        
            Options_Equipment = new SelectList(equipment, "Id", "Name");  
            return Page();
        }

        public async Task<JsonResult> OnPostUpdateExerciseCount(int[] equipmentIds)
        {
            IEnumerable<Models.Exercise> exercises = await _exerciseDb.GetExercisesFromRequiredEquipment(equipmentIds);
            return new JsonResult(exercises.Count());
        }
    }
}