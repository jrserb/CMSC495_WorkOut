using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WorkoutGen.Data.Services.Equipment;
using WorkoutGen.Data.Services.Exercise;
using WorkoutGen.Data.Services.MuscleGroup;
using WorkoutGen.Data.Session;

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
        public int[] MuscleGroupIds { get; set; }
        public SelectList Options_Equipment { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            int[] equipmentIds = HttpContext.Session.Get<int[]>("equipment");
            int[] muscleGroupIds = HttpContext.Session.Get<int[]>("muscle_groups");

            if (muscleGroupIds == null)
            {
                return RedirectToPage("/MuscleGroup/Index");
            }

            if (equipmentIds != null)
            {
                IEnumerable<Models.Exercise> exercises = await _exerciseDb.GetExercisesFromRequiredEquipment(muscleGroupIds, equipmentIds);
                ExerciseCount = exercises.Count();

                var equipment = await _equipmentDb.GetEquipment(equipmentIds);
                SetEquipmentDropDown(equipment);
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int[] muscleGroupIds)
        {
            // Save the muscle groups into session
            HttpContext.Session.Set<int[]>("muscle_groups", muscleGroupIds);

            MuscleGroupIds = muscleGroupIds;

            MuscleGroups = await _muscleGroupDb.GetMuscleGroups(muscleGroupIds);
            IEnumerable<Models.Equipment> equipment;
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


                HttpContext.Session.Set<int[]>("equipment", fullEquipmentIds);
            }

            SetEquipmentDropDown(equipment);
            return Page();
        }

        public async Task<JsonResult> OnPostUpdateExerciseCount(int[] muscleGroupIds, int[] equipmentIds)
        {
            IEnumerable<Models.Exercise> exercises = await _exerciseDb.GetExercisesFromRequiredEquipment(muscleGroupIds, equipmentIds);
            return new JsonResult(exercises.Count());
        }

        // Gets all the equipment records and binds it to the select list object
        // The select list object gets binded to the select 2 drop down
        public void SetEquipmentDropDown(IEnumerable<Models.Equipment> equipment)
        {
            Options_Equipment = new SelectList(equipment, "Id", "Name");
        }
    }
}